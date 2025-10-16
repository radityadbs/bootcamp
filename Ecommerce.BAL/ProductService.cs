using System.Runtime.InteropServices;
using Ecommerce.Core.Contracts;
using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities;
using Ecommerce.DAL.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BAL
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        //create
        public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                ImagePath = createProductDto.ImagePath,
                CategoryId = createProductDto.CategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.ImagePath,
                product.CreatedAt,
                product.UpdatedAt,
                product.CategoryId
            );
        }

        // get all
        public async Task<(IReadOnlyList<ProductDto> Items, int TotalCount)> GetAllAsync(
            string? search,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            int page,
            int pageSize
        )
        {
            var query = _db.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search)
                    || (p.Description != null && p.Description.Contains(search))
                );
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            query = query.OrderBy(p => p.CreatedAt);

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto(
                    p.Id,
                    p.Name,
                    p.Category!.Name,
                    p.Price,
                    p.Stock,
                    p.ImagePath,
                    p.CreatedAt,
                    p.UpdatedAt,
                    p.CategoryId
                ))
                .ToListAsync();

            return (products, totalCount);
        }

        // get by id / details
        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _db
                .Products.Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return null;

            return new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.ImagePath,
                product.CreatedAt,
                product.UpdatedAt,
                product.CategoryId
            );
        }

        // update
        public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _db
                .Products.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return null;

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.Stock = updateProductDto.Stock;
            product.ImagePath = updateProductDto.ImagePath;
            product.CategoryId = updateProductDto.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.ImagePath,
                product.CreatedAt,
                product.UpdatedAt,
                product.CategoryId
            );
        }

        // delete
        public async Task DeleteAsync(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
                return;
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        // get categories
        public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync()
        {
            return await _db
                .Categories.AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new CategoryDto(c.Id, c.Name))
                .ToListAsync();
        }
    }
}
