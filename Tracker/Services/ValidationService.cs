using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Tracker.Controllers.Validation;

namespace Tracker.Services
{
    public class ValidationService
    {
        private readonly string _categoryNamePattern;
        private readonly string _newCategoryNamePattern;
        private readonly string _categoryIdPattern;

        public ValidationService(IOptions<ValidationPatterns> validationPatterns)
        {
            _categoryNamePattern = validationPatterns.Value.SessionController_CreateSessionAsync_categoryName;
            _newCategoryNamePattern = validationPatterns.Value.CategoryController_RenameCategoryAsync_newName;
            _categoryIdPattern = validationPatterns.Value.CategoryController_RenameCategoryAsync_id;
        }

        public bool ValidateCategoryName(string categoryName)
        {
            return Regex.IsMatch(categoryName, _categoryNamePattern);
        }

        public bool ValidateNewCategoryName(string newCategoryName)
        {
            return Regex.IsMatch(newCategoryName, _newCategoryNamePattern);
        }
        public bool ValidateCategoryId(int id)
        {
            return Regex.IsMatch(id.ToString(), _categoryIdPattern);
        }
    }
}
