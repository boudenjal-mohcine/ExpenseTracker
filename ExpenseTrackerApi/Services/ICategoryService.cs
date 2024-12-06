using ExpenseTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryByIdAsync(string id);
        Task<IEnumerable<Category>> GetAllCategoriesUserAsync(string userId);
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);

    }
}
