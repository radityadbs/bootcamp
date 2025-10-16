namespace Ecommerce.Services
{
    public record ProductRequest(
        Guid ProductId,
        string ProductName,
        decimal Price,
        Guid ProductTypeId
    );
}
