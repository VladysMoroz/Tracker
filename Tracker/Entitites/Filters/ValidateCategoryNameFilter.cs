using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tracker.Services;

namespace Tracker.Entitites.Filters
{
    public class ValidateCategoryNameFilter : IActionFilter
    {
        private readonly ValidationService _validationService;

        public ValidateCategoryNameFilter(ValidationService validationService)
        {
            _validationService = validationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("name", out var value) && value is string name)
            {
                var validationResult = _validationService.Validate("categoryName", name);
                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(validationResult.ErrorMessage);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
