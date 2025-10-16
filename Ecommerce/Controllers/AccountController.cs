using System.Security.Claims;
using System.Text;
using Ecommerce.Models;
using Ecommerce.Services.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ecommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Auth/Login", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Login gagal.");
                return View(model);
            }

            var responseData = await response.Content.ReadAsStringAsync();

            // === Penting: pola respons API-mu adalah BaseResponse<T> (lihat ProductApiService) ===
            var auth = JsonConvert.DeserializeObject<AuthResponse>(responseData); // <—

            if (auth == null || !auth.Status || auth.Data == null)
            {
                ModelState.AddModelError("", auth?.Message ?? "Login gagal.");
                return View(model);
            }

            // Simpan token & info user di Session
            HttpContext.Session.SetString("JWToken", auth.Data.AccessToken);
            HttpContext.Session.SetString("UserName", auth.Data.Username ?? model.UserName);
            HttpContext.Session.SetString("IsAdmin", (auth.Data.IsAdmin).ToString());
            HttpContext.Session.SetString("RefreshToken", auth.Data.RefreshToken ?? "");

            // ==== INI KUNCI: buat cookie sign-in agar [Authorize] di MVC tidak redirect ke Login ====
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, auth.Data.Username ?? model.UserName),
                // Samakan dengan yang dipakai di controller: [Authorize(Roles="Admin")]
                new Claim(ClaimTypes.Role, auth.Data.IsAdmin ? "Admin" : "User"),
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal); // ← cookie dibuat di sini

            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // (opsional) kasih tahu API untuk invalidasi refresh token
            var refreshToken = HttpContext.Session.GetString("RefreshToken");
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var json = JsonConvert.SerializeObject(new { refreshToken });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("Auth/Logout", content);
            }

            // Hapus cookie autentikasi + session
            await HttpContext.SignOutAsync("CookieAuth");
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Login));
        }
    }
}
