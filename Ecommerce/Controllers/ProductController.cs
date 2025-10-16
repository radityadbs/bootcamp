using Ecommerce.Models;
using Ecommerce.Services;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class ProductController : Controller
    {
        private readonly ProductApiService _apiService;

        public ProductController(ProductApiService apiService)
        {
            _apiService = apiService;
        }

        // ================================================================
        // GET: /Product (✅ dengan pagination)
        // ================================================================
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                var products = await _apiService.GetAllProductsAsync();
                var totalItems = products.Count();
                const int pageSize = 10;

                if (totalItems == 0)
                {
                    return View(
                        new PagedProductVm
                        {
                            Products = new List<ProductResponse>(),
                            CurrentPage = 1,
                            TotalPages = 1,
                        }
                    );
                }

                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                if (page > totalPages)
                    page = totalPages;

                var pagedData = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var model = new PagedProductVm
                {
                    Products = pagedData,
                    CurrentPage = page,
                    TotalPages = totalPages,
                };
                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                // kalau token expired / belum login
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Terjadi kesalahan: {ex.Message}";
                return View(
                    new PagedProductVm
                    {
                        Products = new List<ProductResponse>(),
                        CurrentPage = 1,
                        TotalPages = 1,
                    }
                );
            }
        }

        // ================================================================
        // GET: /Product/Create
        // ================================================================
        [HttpGet]
        [Authorize(Roles = "Admin")] // ✅ gunakan "Admin", bukan "IsAdmin"
        public async Task<IActionResult> Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWToken")))
                return RedirectToAction("Login", "Account");

            var productTypes = await _apiService.GetProductTypesAsync();
            var vm = new ProductFormVm
            {
                ProductTypes = productTypes.Select(pt => new SelectListItem
                {
                    Value = pt.ProductTypeId.ToString(),
                    Text = pt.ProductTypeName,
                }),
            };
            return View(vm);
        }

        // ================================================================
        // POST: /Product/Create
        // ================================================================
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductFormVm vm)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWToken")))
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                var productTypes = await _apiService.GetProductTypesAsync();
                vm.ProductTypes = productTypes.Select(pt => new SelectListItem
                {
                    Value = pt.ProductTypeId.ToString(),
                    Text = pt.ProductTypeName,
                });
                return View(vm);
            }

            var request = new ProductRequest(
                Guid.Empty,
                vm.ProductName,
                vm.Price,
                vm.ProductTypeId
            );
            bool result = await _apiService.CreateProductAsync(request);

            TempData[result ? "Success" : "Error"] = result
                ? "Produk berhasil ditambahkan."
                : "Gagal menambah produk.";
            return RedirectToAction(nameof(Index));
        }

        // ================================================================
        // GET: /Product/Edit/{id}
        // ================================================================
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _apiService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Product tidak ditemukan.";
                return RedirectToAction(nameof(Index));
            }

            var productTypes = await _apiService.GetProductTypesAsync();
            var vm = new ProductFormVm
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                ProductTypeId = product.ProductTypeId,
                ProductTypes = new SelectList(
                    productTypes,
                    "ProductTypeId",
                    "ProductTypeName",
                    product.ProductTypeId
                ),
            };

            return View(vm);
        }

        // ================================================================
        // POST: /Product/Edit/{id}
        // ================================================================
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, ProductFormVm vm)
        {
            if (!ModelState.IsValid)
            {
                var productTypes = await _apiService.GetProductTypesAsync();
                vm.ProductTypes = productTypes.Select(pt => new SelectListItem
                {
                    Value = pt.ProductTypeId.ToString(),
                    Text = pt.ProductTypeName,
                    Selected = pt.ProductTypeId == vm.ProductTypeId,
                });
                return View(vm);
            }

            var request = new ProductRequest(id, vm.ProductName, vm.Price, vm.ProductTypeId);
            bool result = await _apiService.UpdateProductAsync(id, request);

            TempData[result ? "Success" : "Error"] = result
                ? "Product berhasil diperbarui."
                : "Gagal memperbarui product.";
            return RedirectToAction(nameof(Index));
        }

        // ================================================================
        // GET: /Product/Details/{id}
        // ================================================================
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _apiService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Product tidak ditemukan.";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // ================================================================
        // AJAX Delete
        // ================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAjax(Guid id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var ok = await _apiService.DeleteProductAsync(id, token);
            if (ok)
                return Json(new { ok = true, message = "Product berhasil dihapus." });

            Response.StatusCode = 400;
            return Json(new { ok = false, message = "Gagal menghapus product." });
        }
    }
}
