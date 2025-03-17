using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tracker.Entitites.Enums;
using Tracker.Entitites.Filters;
using Tracker.Interfaces.ServiceInterfaces;
using Tracker.Services;

namespace Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        //private readonly ValidationService _validationService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //[ActivatorUtilitiesConstructor]
        //public CategoryController(ICategoryService categoryService, ValidationService validationService)
        //{
        //    _categoryService = categoryService;
        //    _validationService = validationService;
        //}

        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidateCategoryNameFilter))]
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategoryAsync(string name)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var categories = await _categoryService.GetAllCategoriesAsync();

            return Ok(categories);
        }

        [ServiceFilter(typeof(ValidateNewNameAndIdFilter))]
        [HttpPut("RenameCategory")]
        public async Task<IActionResult> RenameCategoryAsync(string newName, int id)
        {
            if (string.IsNullOrEmpty(newName))
            {
                return BadRequest("New category name cannot be empty.");
            }

            var renamedCategory = await _categoryService.RenameCategoryAsync(newName, id);
            return Ok(renamedCategory);
        }

        [Authorize(Policy = "UserPolicy")]
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
