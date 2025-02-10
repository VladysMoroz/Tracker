using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tracker.Entitites;
using Tracker.Entitites.Enums;
using Tracker.Interfaces;

namespace Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionController(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        [HttpPost("AddSession")]
        public async Task<IActionResult> CreateSessionAsync([FromBody] Categories category)
        {
            // check if session was opened before, it must return an object which wasn't closed
            var session = await _sessionRepository.GetActiveSessionAsync(category);

            if (session is null)
            {
                // in case session existed, we would close it 
                await _sessionRepository.FinishSessionAsync(session);
                return Ok("The old session was successfully closed");
            }
            else
            {
                var sessionForCreating = await _sessionRepository.CreateSessionAsync(category);
                // method which links category to the session 

                return Ok(sessionForCreating);
            }
        }
    }
}
