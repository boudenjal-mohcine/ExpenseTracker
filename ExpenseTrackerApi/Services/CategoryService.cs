using ExpenseTracker.Data;
using ExpenseTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> GetCategoryByIdAsync(string id) =>
            await _categoryRepository.GetCategoryByIdAsync(id);

        public async Task<IEnumerable<Category>> GetAllCategoriesUserAsync(string userId) =>
            await _categoryRepository.GetAllCategoriesUserAsync(userId);

        public async Task CreateCategoryAsync(Category category) =>
            await _categoryRepository.CreateCategoryAsync(category);
    }
}
