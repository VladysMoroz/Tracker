using Microsoft.EntityFrameworkCore;
using Tracker.Entitites;
using Tracker.Interfaces.RepositoryInterfaces;
using Tracker.Repositories;

namespace Tracker.DatabaseCatalog.Repositories
{
    public class GoalRepository : IGoalRepository
    {
        private readonly TrackerDbContext _dbContext;

        public GoalRepository(TrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Goal> CreateGoalAsync(Goal goal)
        {
            var createdGoal = await _dbContext.Goals.AddAsync(goal);

            return createdGoal.Entity;
        }

        public async Task<bool> DeleteGoalAsync(int id)
        {
            var GoalForDelete = await _dbContext.Goals.FirstOrDefaultAsync(x => x.Id == id);

            if (GoalForDelete != null)
            {
                _dbContext.Goals.Remove(GoalForDelete);
                _dbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<Goal> EditGoalAsync(Goal goal)
        {
            var GoalForUpdating = await _dbContext.Goals.FirstOrDefaultAsync(x => x.Id == goal.Id);

            if (GoalForUpdating is null)
            {
                throw new Exception("Goal was not found");
            }

            GoalForUpdating = goal;
            _dbContext.SaveChangesAsync();

            return GoalForUpdating;

        }


        public async Task<List<Goal>> GetAllGoalsAsync()
        {
            var goals = await _dbContext.Goals.ToListAsync();

            return goals;
        }
    }
}
