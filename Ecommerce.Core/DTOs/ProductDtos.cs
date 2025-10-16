using System.Security;

namespace Ecommerce.Core.DTOs
{
    public record ProductDto(
        int Id,
        string Name,
        string? Description,
        decimal Price,
        int Stock,
        string? ImagePath,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        int CategoryId
    );

    public class CreateProductDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImagePath { get; set; }
        public int CategoryId { get; set; }
    }

    public class UpdateProductDto : CreateProductDto
    {
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public record CategoryDto(int id, string Name);
}
