using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Tracker.Controllers.Validation;
using Tracker.Entitites.Enums;
using Tracker.Entitites.Filters;

namespace Tracker.Services
{
    public class ValidationService
    {
        private readonly Dictionary<string, string> _validationPatterns;

        public ValidationService(IOptions<ValidationPatterns> validationPatterns)
        {
            _validationPatterns = new Dictionary<string, string>
        {
            { "categoryName", validationPatterns.Value.SessionController_CreateSessionAsync_categoryName },
            { "newCategoryName", validationPatterns.Value.CategoryController_RenameCategoryAsync_newName },
            { "categoryId", validationPatterns.Value.CategoryController_RenameCategoryAsync_id }
        };
        }

        // Замість того, щоб просто повертати false ( тип bool раніше ), створений клас, що містить текст помилки + тип
        public ValidationResult Validate(string key, string value)
        {
            if (!_validationPatterns.TryGetValue(key, out var pattern))
                throw new ArgumentException($"Validation pattern for '{key}' not found.");

            var isValid = Regex.IsMatch(value, pattern);
            return isValid ? ValidationResult.Success : ValidationResult.Failure($"Параметр {key} не відповідає вимогам.");
        }

        public class ValidationResult
        { 
            public static ValidationResult Success => new ValidationResult { IsValid = true };
            public static ValidationResult Failure(string message) => new ValidationResult { IsValid = false, ErrorMessage = message };

            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }
        }

        public bool ValidateFilter(Filter filter)
        {
            if (filter.Quantity <= 0)
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(OptionsForDisplayingStats), filter.Option))
            {
                return false;
            }

            return true;
        }

        //public bool Validate(string key, string value)
        //{
        //    if (!_validationPatterns.TryGetValue(key, out var pattern))
        //        throw new ArgumentException($"Validation pattern for '{key}' not found.");

        //    return Regex.IsMatch(value, pattern);
        //}
    }
}
