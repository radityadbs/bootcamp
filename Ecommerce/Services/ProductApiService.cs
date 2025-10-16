using System.Net.Http.Headers;
using System.Net.Http.Json;
using Ecommerce.Models;
using Ecommerce.Services;
using Newtonsoft.Json;

public class ProductApiService
{
    private readonly HttpClient _client;
    private readonly IHttpContextAccessor _contextAccessor;

    public ProductApiService(
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor contextAccessor
    )
    {
        _client = httpClientFactory.CreateClient("ApiClient");
        _contextAccessor = contextAccessor;
    }

    //TOKEN DISETIAP REQUEST//
    private void AddToken()
    {
        var token = _contextAccessor.HttpContext?.Session.GetString("JWToken");
        _client.DefaultRequestHeaders.Authorization = null;
        if (!string.IsNullOrEmpty(token))
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );
    }

    // ============================================================
    // ✅ GET: /api/Product/GetAllProduct
    // Sekarang menerima token dari controller
    // ============================================================
    public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
    {
        AddToken();
        var response = await _client.GetAsync("Product/GetAllProduct");
        if (!response.IsSuccessStatusCode)
            throw new Exception("Gagal mengambil data produk dari API");

        var json = await response.Content.ReadAsStringAsync();
        var baseResponse = JsonConvert.DeserializeObject<
            BaseResponse<IEnumerable<ProductResponse>>
        >(json);
        return baseResponse?.Data ?? new List<ProductResponse>();
    }

    // ============================================================
    // GET BY ID
    // ============================================================
    public async Task<ProductResponse?> GetProductByIdAsync(Guid id)
    {
        AddToken();
        var res = await _client.GetAsync($"Product/GetProductById?id={id}");
        var json = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
        {
            Console.WriteLine($"❌ [GetProductByIdAsync] API Error: {res.StatusCode}");
            Console.WriteLine($"Response Body: {json}");
            return null;
        }

        var baseResponse = JsonConvert.DeserializeObject<BaseResponse<ProductResponse>>(json);
        return baseResponse?.Data;
    }

    // ============================================================
    // CREATE PRODUCT
    // ============================================================
    public async Task<bool> CreateProductAsync(ProductRequest product)
    {
        AddToken();
        var result = await _client.PostAsJsonAsync("Product/AddProduct", product);
        var json = await result.Content.ReadAsStringAsync();

        if (!result.IsSuccessStatusCode)
        {
            Console.WriteLine($"❌ [CreateProductAsync] API Error: {result.StatusCode}");
            Console.WriteLine($"Response: {json}");
        }

        return result.IsSuccessStatusCode;
    }

    // ============================================================
    // UPDATE PRODUCT
    // ============================================================
    public async Task<bool> UpdateProductAsync(Guid id, ProductRequest product)
    {
        AddToken();
        var result = await _client.PutAsJsonAsync($"Product/EditProduct?id={id}", product);
        var json = await result.Content.ReadAsStringAsync();

        if (!result.IsSuccessStatusCode)
        {
            Console.WriteLine($"❌ [UpdateProductAsync] API Error: {result.StatusCode}");
            Console.WriteLine($"Response: {json}");
        }

        return result.IsSuccessStatusCode;
    }

    // ============================================================
    // ✅ DELETE PRODUCT (Sekarang pakai token)
    // ============================================================
    public async Task<bool> DeleteProductAsync(Guid id, string? token = null)
    {
        AddToken();
        var response = await _client.DeleteAsync($"Product/DeleteProduct?id={id}");
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"❌ [DeleteProductAsync] API Error: {response.StatusCode}");
            Console.WriteLine($"Response: {json}");
        }
        ;
        return response.IsSuccessStatusCode;
    }

    // ============================================================
    // GET PRODUCT TYPES
    // ============================================================
    public async Task<List<ProductTypeResponse>> GetProductTypesAsync()
    {
        AddToken();
        var response = await _client.GetFromJsonAsync<BaseResponse<List<ProductTypeResponse>>>(
            "ProductType/GetAllProductType"
        );
        return (response != null && response.Status && response.Data != null)
            ? response.Data
            : new List<ProductTypeResponse>();
    }
}
