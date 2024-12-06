using ExpenseTracker.Models;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // Register a new user
        [HttpPost("register")]
        public async Task<ActionResult> Register(User user)
        {
            var existingUser = await _userService.GetUserByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                return Conflict("Username already exists.");
            }

            await _userService.CreateUserAsync(user);
            return Ok(new
            {
                Message = "Register successful",
                User = new
                {
                    Id = user.Id.ToString(),
                    Username = user.Username,
                    MaxMonthlyExpenses = user.MaxMonthlyExpenses,
                    CurrentMonthlyExpenses = 0

                }
            });
        }

        // Login
        [HttpPost("login")]
        public async Task<ActionResult> Login(User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var existingUser = await _userService.GetUserByUsernameAsync(user.Username);

            if (existingUser == null || user.Password != existingUser.Password)
            {
                return Unauthorized("Invalid username or password.");
            }

            var CurrentMonthlyExpenses = existingUser.Expenses?.Sum(expense => expense.Amount);
            return Ok(new
            {
                Message = "Login successful",
                User = new
                {
                    Id = existingUser.Id.ToString(),
                    Username = existingUser.Username,
                    MaxMonthlyExpenses = existingUser.MaxMonthlyExpenses,
                    CurrentMonthlyExpenses = CurrentMonthlyExpenses ?? 0

                }
            });
        }
    }
}
