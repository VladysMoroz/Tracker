using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tracker.Services;

namespace Tracker.Entitites.Filters
{
    public class ValidateNewNameAndIdFilter : IActionFilter
    {
        private readonly ValidationService _validationService;

        public ValidateNewNameAndIdFilter(ValidationService validationService)
        {
            _validationService = validationService;
        }

        // Перед викликом метода контролера
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("newName", out var newNameValue) && newNameValue is string newName)
            {
                var validationResult = _validationService.Validate("categoryName", newName);
                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(validationResult.ErrorMessage);
                }
            }

            if (context.ActionArguments.TryGetValue("id", out var idValue) && idValue is int id)
            {
                var validationResult = _validationService.Validate("categoryId", id.ToString());
                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(validationResult.ErrorMessage);
                }
            }
        }

        // після виклику метода контролера
        public void OnActionExecuted(ActionExecutedContext context)   { }
    }
}
