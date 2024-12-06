using ExpenseTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByIdAsync(string id);
        Task<IEnumerable<Category>> GetAllCategoriesUserAsync(string userId);
        Task CreateCategoryAsync(Category category);
    }
}
