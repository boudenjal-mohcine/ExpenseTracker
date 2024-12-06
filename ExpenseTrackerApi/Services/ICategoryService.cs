using ExpenseTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryByIdAsync(string id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task CreateCategoryAsync(Category category);
    }
}
