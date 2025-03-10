using Tracker.Entitites;
using Tracker.Interfaces.RepositoryInterfaces;
using Tracker.Interfaces.ServiceInterfaces;

namespace Tracker.Services
{
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;

        public GoalService(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public async Task<Goal> CreateGoalAsync(Goal goal)
        {
            var newGoal = await _goalRepository.CreateGoalAsync(goal);

            return newGoal;
        }

        public async Task<bool> DeleteGoalAsync(int id)
        {
            var result = await _goalRepository.DeleteGoalAsync(id);

            return result;
        }

        public async Task<Goal> EditGoalAsync(Goal goal)
        {
            var updatedGoal = await _goalRepository.EditGoalAsync(goal);

            return updatedGoal;
        }

        public async Task<List<Goal>> GetAllGoalsAsync()
        {
            var goals = await _goalRepository.GetAllGoalsAsync();

            return goals;
        }
    }
}
