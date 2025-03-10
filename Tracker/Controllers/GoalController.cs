using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tracker.Entitites;
using Tracker.Entitites.ViewModels;
using Tracker.Interfaces.ServiceInterfaces;

namespace Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalService _goalService;
        private readonly IMapper _mapper;

        public GoalController(IGoalService goalService, IMapper mapper)
        {
            _goalService = goalService;
            _mapper = mapper;
        }

        [HttpGet("GetAllGoals")]
        public async Task<IActionResult> GetAllGoalsAsync()
        {
            var goals = await _goalService.GetAllGoalsAsync();
            if(goals is null)
            {
                return BadRequest("Method has returned nullable value");
            }

            return Ok(goals);
        }

        [HttpPost("CreateGoal")]
        public async Task<IActionResult> CreateGoalAsync([FromBody] GoalForCreatingViewModel goalViewModel)
        {
            var goal = _mapper.Map<GoalForCreatingViewModel, Goal>(goalViewModel);

            var createdGoal = await _goalService.CreateGoalAsync(goal);

            if(createdGoal is null)
            {
                return BadRequest("Goal has not been successfully created");
            }

            return Ok(createdGoal);
        }

        [HttpPut("EditGoal")]
        public async Task<IActionResult> EditGoalAsync([FromBody] GoalForEditingViewModel goalViewModel)
        {
            var goal = _mapper.Map<GoalForEditingViewModel, Goal>(goalViewModel);

            var editedGoal = await _goalService.EditGoalAsync(goal);

            if (editedGoal is null)
            {
                return BadRequest("Goal was not successfully edited");
            }

            return Ok(editedGoal);
        }

        [HttpDelete("DeleteGoal")]
        public async Task<IActionResult> DeleteGoalAsync(int id)
        {
            var result = await _goalService.DeleteGoalAsync(id);

            if (result)
            {
                return Ok("Goal has been deleted");
            }
            else
            {
                return BadRequest("Something has gone wrong");
            }
        }
    }
}
