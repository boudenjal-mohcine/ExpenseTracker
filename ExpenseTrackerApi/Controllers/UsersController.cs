using ExpenseTracker.Models;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        // Endpoint: Register a new user
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

        // Endpoint: Login
        [HttpPost("login")]
        public async Task<ActionResult> Login(string username, string password)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
           
            // Verify password
            if (user == null)
            {
                    return Unauthorized("Invalid username or password.");
            }

            return Ok(new { Message = "Login successful", User = user });
        }

        // Endpoint: Get a user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
