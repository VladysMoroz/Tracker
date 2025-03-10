using Tracker.Entitites;
using Tracker.Entitites.Enums;
using Tracker.Entitites.Filters;
using Tracker.Interfaces.RepositoryInterfaces;
using Tracker.Interfaces.ServiceInterfaces;

namespace Tracker.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<Session> CreateSessionAsync(string categoryName)
        {
            // check if session was opened before, it must return an object which wasn't closed
            var session = await _sessionRepository.GetActiveSessionAsync(categoryName);

            if (session is not null)
            {
                // in case session existed, we would close it 
                var finishedSession = await _sessionRepository.FinishSessionAsync(session);
                return finishedSession;
            }
            else
            {
                var sessionForCreating = await _sessionRepository.CreateSessionAsync(categoryName);
                // method which links category to the session 

                return sessionForCreating;
            }
        }

        public async Task<List<Session>> GetSessionsForStatisticAsync(Filter filter)
        {
            var sessions = await _sessionRepository.GetSessionsForStatisticAsync(filter);

            return sessions;
        }
    }
}
