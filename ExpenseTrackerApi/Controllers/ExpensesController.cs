using ExpenseTracker.Models;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;


        public ExpensesController(IExpenseService expenseService, ICategoryService categoryService, IUserService userService)
        {
            _expenseService = expenseService;
            _categoryService = categoryService;
            _userService = userService;
        }

        // Endpoint: Get all expenses for a user
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetExpensesByUserId(string userId)
        {
            // Fetch all expenses for the user
            var expenses = await _expenseService.GetExpensesByUserIdAsync(userId);

            if (expenses == null || !expenses.Any())
            {
                return NotFound("No expenses found for the user.");
            }

            // Map the expenses to return only required fields
            var result = expenses.Select(expense => new
            {
                Id = expense.Id.ToString(),
                expense.Description,
                expense.Amount,
                expense.Date,
                CategoryId = expense.CategoryId?.ToString()
            });

            return Ok(result);
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

            var returnExpense = new
            {
                Id = expense.Id.ToString(),
                expense.Description,
                expense.Amount,
                expense.Date,
                CategoryId = expense.CategoryId?.ToString()
            };

            return Ok(returnExpense);
        }

        // Endpoint: Create a new expense
        [HttpPost("{userId}/{categoryId}")]
        public async Task<ActionResult> CreateExpense(string userId, string categoryId, Expense expense)
        {
            // Validate the user
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Validate the category
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            // Associate the expense with the user and category
            expense.CategoryId = categoryId;

            expense.UserId = userId;

            // Save the new expense
            await _expenseService.CreateExpenseAsync(expense);

            // Add the expense to the user's list of expenses
            user.Expenses.Add(expense);
            category.Expenses.Add(expense);


            // Update the user and category in the database
            await _userService.UpdateUserAsync(user);
            await _categoryService.UpdateCategoryAsync(category);

            // Calculate total monthly expenses
            var currentMonthExpenses = user.Expenses
                .Where(e => e.Date.Month == DateTime.Now.Month && e.Date.Year == DateTime.Now.Year)
                .Sum(e => e.Amount);

            var newTotal = currentMonthExpenses + expense.Amount;

            // Check if the user exceeds the monthly limit
            bool hasExceededLimit = newTotal > user.MaxMonthlyExpenses;

            var returnExpense = new
            {
                Id = expense.Id.ToString(),
                expense.Description,
                expense.Amount,
                expense.Date,
                CategoryId = expense.CategoryId?.ToString(),
                expenseState = !hasExceededLimit
            };

            return Ok(returnExpense);
        }


        // Endpoint: Delete an expense by ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpense(string id)
        {
            // Fetch the expense by ID to find the associated category and user
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound("Expense not found.");
            }

            // Remove the expense from the corresponding category
            var category = await _categoryService.GetCategoryByIdAsync(expense.CategoryId.ToString());
            if (category != null)
            {
                category.Expenses.RemoveAll(e => e.Id == expense.Id);
                await _categoryService.UpdateCategoryAsync(category);
            }

            // Remove the expense from the corresponding user
            var user = await _userService.GetUserByIdAsync(expense.UserId);
            if (user != null)
            {
                user.Expenses.RemoveAll(e => e.Id == expense.Id);
                await _userService.UpdateUserAsync(user);
            }

            // Delete the expense from the expense service/database
            await _expenseService.DeleteExpenseAsync(id);

            return NoContent();
        }

    }
}
