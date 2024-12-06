using ExpenseTracker.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryRepository(IMongoDatabase database)
        {
            _categories = database.GetCollection<Category>("Categories");
        }

        public async Task<Category> GetCategoryByIdAsync(string id) =>
            await _categories.Find(category => category.Id.ToString() == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync() =>
            await _categories.Find(_ => true).ToListAsync();

        public async Task CreateCategoryAsync(Category category) =>
            await _categories.InsertOneAsync(category);
    }
}
