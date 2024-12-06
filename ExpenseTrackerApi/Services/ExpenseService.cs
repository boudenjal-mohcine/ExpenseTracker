using ExpenseTracker.Data;
using ExpenseTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(string userId) =>
            await _expenseRepository.GetAllExpensesByUserIdAsync(userId);

        public async Task<Expense> GetExpenseByIdAsync(string id) =>
            await _expenseRepository.GetExpenseByIdAsync(id);

        public async Task CreateExpenseAsync(Expense expense) =>
            await _expenseRepository.CreateExpenseAsync(expense);

        public async Task DeleteExpenseAsync(string id) =>
            await _expenseRepository.DeleteExpenseAsync(id);
    }
}
