using Microsoft.EntityFrameworkCore;
using MVC_1.Data;
using MVC_1.Models;

namespace MVC_1.Repositories.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Category model)
        {
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var model = await FindByIdAsync(id);
            if (model != null)
            {
                _context.Categories.Remove(model);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Category?> FindByIdAsync(string id){
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> FindByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }

        public async Task UpdateAsync(Category model)
        {
            _context.Categories.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
