using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.ViewModels
{
    public class ProductFormVm
    {
        public Guid? ProductId { get; set; } // null pada Create

        [Required, StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price cannot be null")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must greater than 0")]
        public decimal Price { get; set; }

        [Required]
        public Guid ProductTypeId { get; set; } // sementara input Text (GUID)

        // 🔽 Tambahkan ini untuk dropdown ProductType
        public IEnumerable<SelectListItem>? ProductTypes { get; set; }
    }
}
