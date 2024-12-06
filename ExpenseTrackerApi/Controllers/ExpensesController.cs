using ExpenseTracker.Models;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        // Endpoint: Get all expenses for a user
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpensesByUserId(string userId)
        {
            var expenses = await _expenseService.GetExpensesByUserIdAsync(userId);
            return Ok(expenses);
        }

        // Endpoint: Get a single expense by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpenseById(string id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        // Endpoint: Create a new expense
        [HttpPost]
        public async Task<ActionResult> CreateExpense(Expense expense)
        {
            await _expenseService.CreateExpenseAsync(expense);
            return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, expense);
        }

        // Endpoint: Delete an expense by ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpense(string id)
        {
            await _expenseService.DeleteExpenseAsync(id);
            return NoContent();
        }
    }
}
