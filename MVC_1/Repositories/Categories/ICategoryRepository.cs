using MVC_1.Models;

namespace MVC_1.Repositories.Categories
{
    public interface ICategoryRepository
    {
        Task CreateAsync(Category model);
        Task UpdateAsync(Category model);
        Task DeleteAsync(string id);
        IQueryable<Category> GetAll();
        Task<Category> FindByIdAsync(string? id);
        Task<Category> FindByNameAsync(string? name);
    }
}
