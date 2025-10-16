using System.Text.Json.Serialization;

namespace Ecommerce.Services
{
    public record ProductTypeResponse(
        [property: JsonPropertyName("productTypeId")] Guid ProductTypeId,
        [property: JsonPropertyName("typeName")] string ProductTypeName
    );
}
