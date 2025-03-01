using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_1.Data;
using MVC_1.Models;
using MVC_1.ViewModels;

namespace MVC_1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .AsEnumerable();

            return View(products);
        }

        public IActionResult Create()
        {
            var categories = _context.Categories.AsEnumerable();

            var viewModel = new CreateProductVM
            {
                Categories = new SelectList(categories, "Id", "Name")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateProductVM viewModel)
        {
            string? fileName = null;

            if (viewModel.File != null)
            {
                fileName = SaveImage(viewModel.File);
            }

            viewModel.Product.Image = fileName;
            viewModel.Product.Id = Guid.NewGuid().ToString();

            _context.Products.Add(viewModel.Product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private string? SaveImage(IFormFile file)
        {
            var types = file.ContentType.Split('/');

            if (types[0] != "image")
            {
                return null;
            }

            var imageName = $"{Guid.NewGuid()}.{types[1]}";

            var imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

            var imagePath = Path.Combine(imagesPath, imageName);

            using (var fileStream = System.IO.File.Create(imagePath))
            {
                using (var formStream = file.OpenReadStream())
                {
                    formStream.CopyTo(fileStream);
                }
            }

            return imageName;
        }

        private string? UpdateImage(IFormFile file, string? oldPath)
        {
            if (file == null)
            {
                if (oldPath != null)
                {
                    var imagesPathdel = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                    var imagePathdel = Path.Combine(imagesPathdel, oldPath);
                    if (System.IO.File.Exists(imagePathdel))
                    {
                        System.IO.File.Delete(imagePathdel);
                    }
                }
                return null;
            }
            var types = file.ContentType.Split('/');

            if (types[0] != "image")
            {
                return null;
            }

            if (oldPath != null)
            {
                var imagesPathdel = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                var imagePathdel = Path.Combine(imagesPathdel, oldPath);
                if (System.IO.File.Exists(imagePathdel))
                {
                    System.IO.File.Delete(imagePathdel);
                }
            }

            var imageName = $"{Guid.NewGuid()}.{types[1]}";
            var imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

            var imagePath = Path.Combine(imagesPath, imageName);

            using (var fileStream = System.IO.File.Create(imagePath))
            {
                using (var formStream = file.OpenReadStream())
                {
                    formStream.CopyTo(fileStream);
                }
            }

            return imageName;
        }

        public IActionResult Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (Product == null)
            {
                return NotFound();
            }

            var categories = _context.Categories.AsEnumerable();

            var viewModel = new EditProductVM
            {
                Categories = new SelectList(categories, "Id", "Name"),
                Product = Product,
                CurrentImage = Product.Image
            };

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditProductVM viewModel)
        {
            string? fileName = null;

            fileName = UpdateImage(viewModel.File, viewModel.CurrentImage);

            viewModel.Product.Image = fileName;

            if (string.IsNullOrEmpty(viewModel.Product.Id))
            {
                return BadRequest("ID продукту відсутнє");
            }

            _context.Products.Update(viewModel.Product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(product.Image))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products", product.Image);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}