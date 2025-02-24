using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tracker.Entitites.Enums;
using Tracker.Interfaces;
using Tracker.Services;

namespace Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ValidationService _validationService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [ActivatorUtilitiesConstructor]
        public CategoryController(ICategoryService categoryService, ValidationService validationService)
        {
            _categoryService = categoryService;
            _validationService = validationService;
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategoryAsync(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var createdCategory = await _categoryService.CreateCategoryAsync(name);

                return Ok(createdCategory);
            }
            else
            {
                throw new ArgumentNullException(nameof(name), "Incorrect variable value \"name\" - it must not be null or empty");
            }
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            return Ok(categories);
        }

        [HttpPut("RenameCategory")]
        public async Task<IActionResult> RenameCategoryAsync(string newName, int id)
        {
            if (string.IsNullOrEmpty(newName))
            {
                return BadRequest("New category name cannot be empty.");
            }

            if (!_validationService.ValidateNewCategoryName(newName))
            {
                return BadRequest("Invalid category name format.");
            }
            if (!_validationService.ValidateCategoryId(id))
            {
                return BadRequest("Invalid category ID. It must be a positive number (1-99).");
            }

            var renamedCategory = await _categoryService.RenameCategoryAsync(newName, id);
            return Ok(renamedCategory);
        }

        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound("The category with the given Id was not found in the database");
            }

            return Ok("Category has been deleted successfully!");
        }
    }
}
