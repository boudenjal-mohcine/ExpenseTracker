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
            return Ok("User registered successfully.");
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

            return Ok(new
            {
                Message = "Login successful",
                User = new
                {
                    Id = existingUser.Id.ToString(),
                    Username = existingUser.Username,
                    Expenses = existingUser.Expenses
                }
            });
        }
    }
}