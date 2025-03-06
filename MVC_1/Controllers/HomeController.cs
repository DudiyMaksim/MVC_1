using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_1.Data;
using MVC_1.Models;
using MVC_1.Repositories.Products;
using MVC_1.ViewModels;
using System.Diagnostics;

namespace MVC_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, AppDbContext context)
        {
            _logger = logger;
            _productRepository = productRepository;
            _context = context;
        }

        public IActionResult Index(string? category)
        {
            var categories = _context.Categories;

            var products = string.IsNullOrEmpty(category)
                ? _productRepository.GetAll().Include(p => p.Category)
                : _productRepository.GetByCategory(category);


            var viewModel = new HomeProductListVM
            {
                Products = products,
                Categories = categories
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        [ActionName("Details")]
        public async Task<IActionResult> ProductDetails(string id)
        {
            Product? product = await _productRepository.FindByIdAsync(id);
            return View("ProductDetails", product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
