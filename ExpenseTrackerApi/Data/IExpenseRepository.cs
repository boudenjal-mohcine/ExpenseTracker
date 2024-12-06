using ExpenseTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> GetAllExpensesByUserIdAsync(string userId);
        Task<Expense> GetExpenseByIdAsync(string id);
        Task CreateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(string id);
    }
}
