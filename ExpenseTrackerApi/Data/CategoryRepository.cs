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

        public async Task<IEnumerable<Category>> GetAllCategoriesUserAsync(string userId)
        {
            return await _categories
                         .Find(category => category.UserId == userId) 
                         .ToListAsync(); 
        }


        public async Task CreateCategoryAsync(Category category) =>
            await _categories.InsertOneAsync(category);

         public async Task UpdateCategoryAsync(Category category)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.Id, category.Id);
            var update = Builders<Category>.Update.Set(c => c.Expenses, category.Expenses);
            await _categories.UpdateOneAsync(filter, update);
        }

    }
}
