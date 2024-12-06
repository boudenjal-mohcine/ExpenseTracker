using ExpenseTracker.Models;
using ExpenseTracker.Services;
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

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Endpoint: Get a category by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // Endpoint: Get all categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // Endpoint: Create a new category
        [HttpPost]
        public async Task<ActionResult> CreateCategory(Category category)
        {
            await _categoryService.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }
    }
}
