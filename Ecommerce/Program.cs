using System.Globalization;
using Ecommerce.Services;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// ======================
// 1️⃣  Services
// ======================

// MVC
builder.Services.AddControllersWithViews();

// HttpContextAccessor (untuk akses Session di service)
builder.Services.AddHttpContextAccessor();

// Session (buat simpan JWT, Username, dsb)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// HttpClient untuk API eksternal (API temanmu)
builder.Services.AddHttpClient(
    "ApiClient",
    client =>
    {
        client.BaseAddress = new Uri("http://localhost:5049/api/"); // ⚠️ ubah sesuai API kamu
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    }
);

// Authentication pakai CookieAuth
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "CookieAuth";
        options.DefaultChallengeScheme = "CookieAuth";
    })
    .AddCookie(
        "CookieAuth",
        options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/Login";
        }
    );

// Scoped services
builder.Services.AddScoped<ProductApiService>();

var app = builder.Build();

// ======================
// 2️⃣  Middleware
// ======================

// Error handler / HSTS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ======================
// 3️⃣  Localization (id-ID)
// ======================
var cultureInfo = new CultureInfo("id-ID");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var supportedCultures = new[] { cultureInfo };
app.UseRequestLocalization(
    new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("id-ID"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures,
    }
);

// ======================
// 4️⃣  Routing & Pipeline
// ======================
app.UseRouting();

app.UseSession(); // harus sebelum Authentication
app.UseAuthentication();
app.UseAuthorization();

// ======================
// 5️⃣  Endpoint
// ======================
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
