using MVC_1.Models;

namespace MVC_1.ViewModels
{
    public class HomeProductListVM
    {
        public IEnumerable<Product> Products { get; set; } = [];
        public IEnumerable<Category> Categories { get; set; } = [];
        public string Category { get; set; } = string.Empty;
        public int PagesCount { get; set; } = 1;
        public int Page { get; set; } = 1;
    }
}
