using Microsoft.AspNetCore.Mvc;
using MVC_1.Data;
using MVC_1.Models;
using MVC_1.Repositories.Categories;

namespace MVC_1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryController)
        {
            _categoryRepository = categoryController;
        }
        public IActionResult Index()
        {
            IQueryable<Category> categories = _categoryRepository.GetAll();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            model.Id = Guid.NewGuid().ToString();

            await _categoryRepository.CreateAsync(model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.FindByIdAsync(Id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category model)
        {
            await _categoryRepository.UpdateAsync(model);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteAsync(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.FindByIdAsync(Id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAsync(Category model)
        {
            if (model.Id == null)
            {
                return NotFound();
            }

            _categoryRepository.DeleteAsync(model.Id);
            return RedirectToAction("Index");
        }
    }
}
