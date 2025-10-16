using Ecommerce.Services;

namespace Ecommerce.ViewModels
{
    public class PagedProductVm
    {
        public IEnumerable<ProductResponse> Products { get; set; } = new List<ProductResponse>();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
