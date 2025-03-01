using Microsoft.AspNetCore.Mvc;
using MVC_1.Data;
using MVC_1.Models;

namespace MVC_1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _context.Categories.AsEnumerable();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            model.Id = Guid.NewGuid().ToString();

            _context.Categories.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(string? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.SingleOrDefault(c => c.Id == Id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category model)
        {
            _context.Categories.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Delete(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.SingleOrDefault(c => c.Id == Id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category model)
        {
            _context.Categories.Remove(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
