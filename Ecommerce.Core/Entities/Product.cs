namespace Ecommerce.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int ProductTypeId { get; set; }
        public string? ProductTypeName { get; set; } // optional jika API kirim nama tipe
    }
}
