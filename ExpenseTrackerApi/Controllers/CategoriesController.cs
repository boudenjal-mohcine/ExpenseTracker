using ExpenseTracker.Models;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        public CategoriesController(ICategoryService categoryService, IUserService userService)
        {
            _categoryService = categoryService;
            _userService = userService;


        }

        // Endpoint: Get a category by ID for a specific user
        [HttpGet("{id}/{userId}")]
        public async Task<ActionResult<Category>> GetCategoryById(string id, string userId)
        {
            // Check if the user exists by the userId
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Category not found or you do not have permission to access it.");
            }

            return Ok(new
            {
                Id = category.Id.ToString(),
                category.Name,
                category.Expenses,
                category.UserId
            });
        }

        // Endpoint: Get all categories for a specific user
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories(string userId)
        {
            // Check if the user exists by the userId
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var categories = await _categoryService.GetAllCategoriesUserAsync(userId);


            return Ok(categories.Select(category => new
            {
                Id = category.Id.ToString(),
                category.Name
                //category.Expenses,
                //category.UserId
            }).ToList());
        }

        // Endpoint: Create a new category
        [HttpPost("{userId}")]
        public async Task<ActionResult<IEnumerable<Category>>> CreateCategory(string userId, Category category)
        {
            // Check if the user exists by the userId
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the category already exists for the user
            if (user.Categories.Any(c => c.Name == category.Name))
            {
                return BadRequest("Category already exists.");
            }

            await _categoryService.CreateCategoryAsync(category);

            // Set the UserId before saving the category
            category.UserId = userId;
            category.User = user;

            user.Categories.Add(category);

            await _userService.UpdateUserAsync(user);


            return Ok(new
            {
                id = category.Id.ToString(),
                userId = category.UserId,
                name = category.Name
            });
        }
    }
}
