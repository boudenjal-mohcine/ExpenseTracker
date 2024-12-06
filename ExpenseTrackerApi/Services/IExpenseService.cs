using ExpenseTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public interface IExpenseService
    {
        Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(string userId);
        Task<Expense> GetExpenseByIdAsync(string id);
        Task CreateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(string id);
    }
}
