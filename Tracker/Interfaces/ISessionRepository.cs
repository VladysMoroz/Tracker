using Tracker.Entitites;
using Tracker.Entitites.Enums;

namespace Tracker.Interfaces
{
    public interface ISessionRepository
    {
        Task<Session> FinishSessionAsync(Session session);
        public Task<Session> CreateSessionAsync(Categories category);
        public Task<Session> GetActiveSessionAsync(Categories category);
    }
}
