using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tracker.Services;

namespace Tracker.Entitites.Filters
{
    public class ValidateFilterAttribute : IActionFilter
    {
        private readonly ValidationService _validationService;
        public ValidateFilterAttribute(ValidationService validationService)
        {
            _validationService = validationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("filter", out var value) && value is Filter filter)
            {
                if (!_validationService.ValidateFilter(filter))
                {
                    context.Result = new BadRequestObjectResult("Некоректний фільтр. Quantity має бути більше 0, а Option повинен мати допустиме значення.");
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult("Фільтр не був переданий або має некоректний формат.");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
