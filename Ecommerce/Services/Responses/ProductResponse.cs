namespace Ecommerce.Services
{
    public record ProductResponse(
        Guid ProductId,
        string ProductName,
        decimal Price,
        string ProductTypeName,
        Guid ProductTypeId
    );
}
