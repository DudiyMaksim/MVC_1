using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_1.Data;
using MVC_1.Models;
using MVC_1.Repositories.Products;
using MVC_1.ViewModels;
using MVC_1.Services;
using System.Diagnostics;
using MVC_1.Settings;

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

        public IActionResult Index(string? category, int page)
        {
            var categories = _context.Categories;

            var products = string.IsNullOrEmpty(category)
                ? _productRepository.GetAll().Include(p => p.Category)
                : _productRepository.GetByCategory(category);


            int pageSize = 3;
            int totalCount = products.Count();
            int pagesCount = (int)Math.Ceiling((double)totalCount / pageSize);
            page = page < 1 || page > pagesCount ? 1 : page;
            products = products.Skip((page - 1) * pageSize).Take(pageSize);

            var cartItems = HttpContext.Session.Get<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey);

            if (cartItems != null)
            {
                foreach (var product in products)
                {
                    product.InCart = cartItems.Select(i => i.ProductId).Contains(product.Id);
                }
            }

                var viewModel = new HomeProductListVM
            {
                Products = products,
                Categories = categories,
                Category = category ?? "",
                PagesCount = pagesCount,
                Page = page
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
