using Tracker.Entitites;

namespace Tracker.Interfaces.ServiceInterfaces
{
    public interface IGoalService
    {
        Task<Goal> CreateGoalAsync(Goal goal);
        Task<bool> DeleteGoalAsync(int id);
        Task<Goal> EditGoalAsync(Goal goal);
        Task<List<Goal>> GetAllGoalsAsync();
    }
}
