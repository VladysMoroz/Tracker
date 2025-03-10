using Tracker.Entitites;
using Tracker.Entitites.Enums;
using Tracker.Entitites.Filters;

namespace Tracker.Interfaces.RepositoryInterfaces
{
    public interface ISessionRepository
    {
        Task<Session> FinishSessionAsync(Session session);
        public Task<Session> CreateSessionAsync(string categoryName);
        public Task<Session> GetActiveSessionAsync(string categoryName);
        public Task<List<Session>> GetSessionsForStatisticAsync(Filter filter);
    }
}
