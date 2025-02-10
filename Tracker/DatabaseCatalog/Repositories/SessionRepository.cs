using Microsoft.EntityFrameworkCore;
using Tracker.Entitites;
using Tracker.Entitites.Enums;
using Tracker.Interfaces;

namespace Tracker.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly TrackerDbContext _dbContext;

        public SessionRepository(TrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Session> CreateSessionAsync(Categories category)
        {
            var dbCategory = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Name == category.ToString());

            var session = new Session { StartSession = DateTime.Now, Category = dbCategory };

            var dbSession = await _dbContext.Sessions.AddAsync(session);
            await _dbContext.SaveChangesAsync();

            return dbSession.Entity;
        }

        public async Task<Session> GetActiveSessionAsync(Categories category)
        {
            var session = await _dbContext.Sessions.Include(s => s.Category).
                Where(x => x.Category.Name == category.ToString() && x.EndSession == null)
                .FirstOrDefaultAsync();

            return session;
        }

        public async Task<Session> FinishSessionAsync(Session session)
        { 
            var dbSession = await _dbContext.Sessions.FirstOrDefaultAsync(s => s.Id == session.Id);
            dbSession.EndSession = DateTime.Now;
            await _dbContext.SaveChangesAsync();

            return dbSession;
        }
    }
}
