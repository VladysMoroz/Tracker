using Tracker.Entitites;
using Tracker.Entitites.Filters;

namespace Tracker.Interfaces
{
    public interface ISessionService
    {
        public Task<Session> CreateSessionAsync(string categoryName);
        public Task<List<Session>> GetSessionsForStatisticAsync(Filter filter);
    }
}
