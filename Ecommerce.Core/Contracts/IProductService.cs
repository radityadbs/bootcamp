using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Contracts
{
    public interface IProductService
    {
        Task<(IReadOnlyList<ProductDto> Items, int TotalCount)> GetAllAsync(
            string? search,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            int page,
            int pageSize
        );

        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductDto createProductDto);
        Task<ProductDto> UpdateAsync(int id, UpdateProductDto updateProductDto);
        Task DeleteAsync(int id);
        Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync();
    }
}
