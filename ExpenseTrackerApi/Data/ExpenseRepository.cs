using ExpenseTracker.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly IMongoCollection<Expense> _expenses;

        public ExpenseRepository(IMongoDatabase database)
        {
            _expenses = database.GetCollection<Expense>("Expenses");
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesByUserIdAsync(string userId) =>
            await _expenses.Find(expense => expense.UserId == userId).ToListAsync();

        public async Task<Expense> GetExpenseByIdAsync(string id) =>
            await _expenses.Find(expense => expense.Id == id).FirstOrDefaultAsync();

        public async Task CreateExpenseAsync(Expense expense) =>
            await _expenses.InsertOneAsync(expense);

        public async Task DeleteExpenseAsync(string id) =>
            await _expenses.DeleteOneAsync(expense => expense.Id == id);
    }
}
