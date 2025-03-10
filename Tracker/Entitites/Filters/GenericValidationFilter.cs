using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Tracker.Services;

namespace Tracker.Entitites.Filters
{
    public class GenericValidationFilter : IActionFilter
    {
        private readonly ValidationService _validationService;

        public GenericValidationFilter(ValidationService validationService)
        {
            _validationService = validationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments)
            {
                string parameterName = argument.Key;
                object parameterValue = argument.Value;

                if (parameterValue is string strValue)
                {
                    var validationResult = _validationService.Validate(parameterName, strValue);
                    if (validationResult.IsValid)
                    {
                        context.Result = new BadRequestObjectResult(validationResult.ErrorMessage);
                        return;
                    }
                }
                else if (parameterValue is int intValue)
                {
                    var validationResult = _validationService.Validate("categoryId", intValue.ToString());
                    if (validationResult.IsValid)
                    {
                        context.Result = new BadRequestObjectResult(validationResult.ErrorMessage);
                        return;
                    }
                }
                else if (parameterValue is Filter filter)
                {
                    if (!_validationService.ValidateFilter(filter))
                    {
                        context.Result = new BadRequestObjectResult("Некоректні значення у фільтрі.");
                        return;
                    }
                }
                else if (parameterValue is not null)
                {
                    // Перевірка всіх властивостей об'єкта, якщо він не null і не простий тип
                    foreach (var property in parameterValue.GetType().GetProperties())
                    {
                        if (property.PropertyType == typeof(string) && property.GetValue(parameterValue) is string propValue)
                        {
                            var validationResult = _validationService.Validate(property.Name, propValue);
                            if (validationResult.IsValid)
                            {
                                context.Result = new BadRequestObjectResult($"Некоректне значення у {property.Name}");
                                return;
                            }
                        }
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
