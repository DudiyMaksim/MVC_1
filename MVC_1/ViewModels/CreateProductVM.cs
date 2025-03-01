using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_1.Models;

namespace MVC_1.ViewModels
{
    public class CreateProductVM
    {
        public Product Product { get; set; } = new();
        public IEnumerable<SelectListItem> Categories { get; set; } = [];

        public IFormFile? File { get; set; }
    }
}
