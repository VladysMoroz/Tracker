using Microsoft.AspNetCore.Mvc;
using Tracker.Entitites.Enums;
using Tracker.Entitites.Filters;
using Tracker.Interfaces;
using Tracker.Services;

namespace Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly ValidationService _validationService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [ActivatorUtilitiesConstructor]
        public SessionController(ISessionService sessionService, ValidationService validationService)
        {
            _sessionService = sessionService;
            _validationService = validationService;
        }

        [HttpPost("AddSession")]
        public async Task<IActionResult> CreateSessionAsync([FromBody] string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("Category name cannot be empty.");
            }

            if (!_validationService.ValidateCategoryName(categoryName))
            {
                return BadRequest("Invalid category name format.");
            }


            var sessionForCreating = await _sessionService.CreateSessionAsync(categoryName);

            return Ok(sessionForCreating);
            
        }

        [HttpPost("GetSessionsForStatistic")]
        public async Task<IActionResult> GetSessionsForStatisticAsync([FromBody]Filter filter)
        {
            if(filter is null)
            {
                return BadRequest("Empty filter was sent to the API");
            }
            else
            {
                var sessions = await _sessionService.GetSessionsForStatisticAsync(filter);
                if(sessions is null)
                {
                    return NotFound("No sessions have been found by defined filter");
                }

                return Ok(sessions);
            }
        }
    }
}
