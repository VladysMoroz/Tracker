using Microsoft.EntityFrameworkCore;
using Tracker.Entitites;
using Tracker.Entitites.Enums;
using Tracker.Entitites.Filters;
using Tracker.Interfaces;

namespace Tracker.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly TrackerDbContext _dbContext;
        public const int WEEK = 7;

        public SessionRepository(TrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Session> CreateSessionAsync(string categoryName)
        {
            var dbCategory = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Name == categoryName);

            var session = new Session { StartSession = DateTime.Now, Category = dbCategory };

            var dbSession = await _dbContext.Sessions.AddAsync(session);
            await _dbContext.SaveChangesAsync();

            return dbSession.Entity;
        }

        public async Task<Session?> GetActiveSessionAsync(string categoryName)
        {
            var session = await _dbContext.Sessions.Include(s => s.Category).
                Where(x => x.Category.Name == categoryName && x.EndSession == null)
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

        //public async Task<List<Session>> GetSessionsForStatisticAsync(OptionsForShowingStatist option, DateTime date)
        //{
        //    var query = _dbContext.Sessions.AsQueryable();

        //    switch (option)
        //    {
        //        case OptionsForShowingStatist.ShowByDay:
        //            query = query.Where(s => s.StartSession.Date == date.Date);
        //            break;

        //        case OptionsForShowingStatist.ShowByWeek:
        //            var startOfWeek = date.Date.AddDays(-(int)date.DayOfWeek + 1); // Початок тижня (понеділок)
        //            var endOfWeek = startOfWeek.AddDays(6); // Кінець тижня (неділя)
        //            query = query.Where(s => s.StartSession.Date >= startOfWeek && s.StartSession.Date <= endOfWeek);
        //            break;

        //        case OptionsForShowingStatist.ShowByMonth:
        //            query = query.Where(s => s.StartSession.Year == date.Year && s.StartSession.Month == date.Month);
        //            break;

        //        case OptionsForShowingStatist.ShowByYear:
        //            query = query.Where(s => s.StartSession.Year == date.Year);
        //            break;

        //        default:
        //            throw new ArgumentException("Invalid statistic filter.");
        //    }

        //    return await query.ToListAsync();
        //}

        public async Task<List<Session>> GetSessionsForStatisticAsync(Filter filter)
        {
            var query = _dbContext.Sessions.AsQueryable();
            var now = DateTime.Now;
            DateTime filteredDate; 

            switch (filter.Option)
            {
                case OptionsForDisplayingStats.Day:
                    filteredDate = now.AddDays(-filter.Quantity);
                    break;

                case OptionsForDisplayingStats.Week:
                    var weeks = WEEK * filter.Quantity;
                    filteredDate = now.AddDays(-weeks);
                    break;

                case OptionsForDisplayingStats.Month:
                    filteredDate = now.AddMonths(-filter.Quantity);
                    break;
                case OptionsForDisplayingStats.Year:
                    filteredDate = now.AddYears(-filter.Quantity);
                    break;

                case OptionsForDisplayingStats.CurrentDay:
                    filteredDate = now.AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);
                    break;
                case OptionsForDisplayingStats.CurrentWeek:
                    filteredDate = now.AddDays(-(int)now.DayOfWeek + 1).AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);
                    // Порядок виконання операторів в методі AddDays(-(int)now.DayOfWeek + 1)
                    /* 1. Виконується дія " now.DayOfWeek " - отримуємо день тижня у форматі DayOfWeek ( Наприклад, сьогодні п'ятниця, отже
                     * буде DayOfWeek.Friday   2. "-(int)date.DayOfWeek " – конвертуємо день тижня в ціле число +інверсія числа ( = -5 у випадку з Friday )  
                     * 3. " -(int)date.DayOfWeek + 1 " - обчислюємо кількість днів, які потрібно відняти від дати, щоб отримати понеділок 
                     * поточного тижня ( -5 + 1 дорівнює -4 ).  4. Виконується запит AddDays(-4) ( = Понеділок ); */
                    break;
                case OptionsForDisplayingStats.CurrentMonth:
                    filteredDate = now.AddDays(-now.Day).AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);
                    break;
                case OptionsForDisplayingStats.CurrentYear:
                    filteredDate = now.AddMonths(-now.Month).AddDays(-now.Day).AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);
                    break;

                default:
                    throw new ArgumentException("Invalid statistic filter.");
            }

            query = query.Where(s => s.StartSession >= filteredDate);

            return await query.ToListAsync();
        }
    }
    
}
