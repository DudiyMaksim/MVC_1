using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_1.Models;

namespace MVC_1.ViewModels
{
    public class EditProductVM
    {
        public Product Product { get; set; } = new();
        public IFormFile? File { get; set; }
        public string? CurrentImage { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; } = [];
    }
}
