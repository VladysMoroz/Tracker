using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Controllers;
using Tracker.Entitites;
using Tracker.Entitites.Enums;
using Tracker.Entitites.Filters;
using Tracker.Interfaces.RepositoryInterfaces;
using Tracker.Services;
using Xunit;

namespace Tracker.Test.Services
{
    public class SessionServiceTest
    {
        private readonly Mock<ISessionRepository> _sessionRepository;
        private readonly SessionService _sut;
        public SessionServiceTest()
        {
            _sessionRepository = new Mock<ISessionRepository>();
            _sut = new SessionService(_sessionRepository.Object);
        }

        public static IEnumerable<object[]> GetFilterTestData()
        {
            return new List<object[]>
        {
            new object[] { new Filter { Option = OptionsForDisplayingStats.Day, Quantity = 1 }, DateTime.Now.AddDays(-1) },
            new object[] { new Filter { Option = OptionsForDisplayingStats.Week, Quantity = 2 }, DateTime.Now.AddDays(-14) },
            new object[] { new Filter { Option = OptionsForDisplayingStats.Month, Quantity = 1 }, DateTime.Now.AddMonths(-1) },
            new object[] { new Filter { Option = OptionsForDisplayingStats.Year, Quantity = 1 }, DateTime.Now.AddYears(-1) },
            new object[] { new Filter { Option = OptionsForDisplayingStats.CurrentDay, Quantity = 0 }, DateTime.Now.Date },
            new object[] { new Filter { Option = OptionsForDisplayingStats.CurrentWeek, Quantity = 0 }, DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1).Date },
            new object[] { new Filter { Option = OptionsForDisplayingStats.CurrentMonth, Quantity = 0 }, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) },
            new object[] { new Filter { Option = OptionsForDisplayingStats.CurrentYear, Quantity = 0 }, new DateTime(DateTime.Now.Year, 1, 1) }
        };
        }

        [Fact]
        public async Task CreateSessionAsync_WhenSessionExists_ClosesAndReturnsExistingSession()
        {
            // Arrange
            var categoryName = "TestCategory";
            var existingSession = new Session { StartSession = DateTime.Now };

            _sessionRepository
                .Setup(repo => repo.GetActiveSessionAsync(categoryName))
                .ReturnsAsync(existingSession); // Імітуємо існуючу активну сесію

            _sessionRepository
                .Setup(repo => repo.FinishSessionAsync(existingSession))
                .ReturnsAsync(existingSession);

            // Act
            var result = await _sut.CreateSessionAsync(categoryName);

            // Assert
            var session = Assert.IsType<Session>(result);
            Assert.NotNull(session);
            Assert.Equal(existingSession.StartSession, session.StartSession);

            _sessionRepository.Verify(repo => repo.FinishSessionAsync(existingSession), Times.Once);
            _sessionRepository.Verify(repo => repo.CreateSessionAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CreateSessionAsync_WhenNoActiveSessions_CreateAndReturnsNewSession()
        {
            // Arrange
            var categoryName = "TestCategory";
            var existingSession = new Session { StartSession = DateTime.Now };

            _sessionRepository
                .Setup(repo => repo.GetActiveSessionAsync(categoryName))
                .ReturnsAsync(null as Session); // Імітуємо існуючу активну сесію

            _sessionRepository
                .Setup(repo => repo.CreateSessionAsync(categoryName))
                .ReturnsAsync(existingSession);

            // Act
            var result = await _sut.CreateSessionAsync(categoryName);

            // Assert
            var session = Assert.IsType<Session>(result);
            Assert.NotNull(session);
            Assert.Equal(existingSession.StartSession, session.StartSession);

            _sessionRepository.Verify(repo => repo.FinishSessionAsync(existingSession), Times.Never);
            _sessionRepository.Verify(repo => repo.CreateSessionAsync(It.IsAny<string>()), Times.Once);
        }


        [Fact]
        public async Task GetSessionsForStatisticAsync_FilterOptionIsByCurrentDay_ListSessionsByCurrentDay()
        {
            // Arrange
            var filter = new Filter
            {
                Option = OptionsForDisplayingStats.CurrentDay,
            };
            var now = DateTime.Now;
            var testSessions = new List<Session>
                {
                    new Session { Id = 1, StartSession = now.AddHours(-2) },   // Сьогодні
                    new Session { Id = 2, StartSession = now.AddHours(-5) },   // Сьогодні
                    new Session { Id = 3, StartSession = now.AddDays(-1) },    // Вчора
                    new Session { Id = 4, StartSession = now.AddDays(-3) },    // Три дні тому
                    new Session { Id = 5, StartSession = now.AddDays(-10) }    // Десять днів тому
                };

            _sessionRepository.Setup(repo => repo.GetSessionsForStatisticAsync(filter))
                    .ReturnsAsync((Filter filter) =>
                    {
                        return testSessions.Where(s => s.StartSession >= now.Date).ToList();
                    });

            // Act
            var result = await _sut.GetSessionsForStatisticAsync(filter);

            // Assert
            var okResult = Assert.IsType<List<Session>>(result);
            Assert.All(result, session => Assert.True(session.StartSession >= now.Date));
            var expectedCount = testSessions.Count(s => s.StartSession >= now.Date);
            Assert.Equal(expectedCount, result.Count);
            Assert.DoesNotContain(result, session => session.StartSession < now.Date);
        }

        [Fact]
        public async Task GetSessionsForStatisticAsync_FilterOptionIsIncorrect_ThrowArgumentException()
        {
            // Arrange
            var filter = new Filter
            {
                Option = (OptionsForDisplayingStats)999,
                Quantity = -12415
            };

            _sessionRepository
                .Setup(repo => repo.GetSessionsForStatisticAsync(filter))
                .ThrowsAsync(new ArgumentException("Invalid statistic filter."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetSessionsForStatisticAsync(filter));

            Assert.Equal("Invalid statistic filter.", exception.Message);
        }

        [Fact]
        public async Task GetSessionsForStatisticAsync_FilterOptionIsByDays_ReturnsListSessionsByDefainDays()
        {
            // Arrange
            var filter = new Filter
            {
                Option = OptionsForDisplayingStats.Day,
                Quantity = 2
            };
            var now = DateTime.Now;
            var testSessions = new List<Session>
                {
                    new Session { Id = 1, StartSession = now.AddHours(-2) },   // Сьогодні
                    new Session { Id = 2, StartSession = now.AddHours(-5) },   // Сьогодні
                    new Session { Id = 3, StartSession = now.AddDays(-1) },    // Вчора
                    new Session { Id = 4, StartSession = now.AddDays(-3) },    // Три дні тому
                    new Session { Id = 5, StartSession = now.AddDays(-10) }    // Десять днів тому
                };
            // Визначаємо, які сесії мають потрапити у вибірку (за останні 2 дні)
            var expectedSessions = testSessions.Where(s => s.StartSession >= now.AddDays(-filter.Quantity))
                                               .ToList();

            _sessionRepository.Setup(repository => repository.GetSessionsForStatisticAsync(filter))
                        .ReturnsAsync((Filter filter) =>
                        {
                            var filteredDate = now.AddDays(-filter.Quantity);
                            return testSessions.Where(s => s.StartSession >= filteredDate).ToList();
                        });

            // Act
            var result = await _sut.GetSessionsForStatisticAsync(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSessions.Count, result.Count);
            Assert.All(result, session => Assert.True(session.StartSession >= now.AddDays(-2), "Session is older than expected range."));
        }

        [Theory]
        [MemberData(nameof(GetFilterTestData))]
        public async Task GetSessionsForStatisticAsync_ShouldReturnFilteredSessions(Filter filter, DateTime expectedDate)
        {
            // Arrange
            var sessions = new List<Session>
        {
            new Session { StartSession = DateTime.Now.AddDays(-10) },
            new Session { StartSession = DateTime.Now.AddDays(-5) },
            new Session { StartSession = DateTime.Now.AddDays(-2) },
            new Session { StartSession = DateTime.Now.AddDays(-1) },
            new Session { StartSession = DateTime.Now }
        };

            _sessionRepository
                .Setup(repo => repo.GetSessionsForStatisticAsync(filter))
                .ReturnsAsync(sessions.Where(s => s.StartSession >= expectedDate).ToList());

            // Act
            var result = await _sut.GetSessionsForStatisticAsync(filter);

            // Assert
            Assert.All(result, session => Assert.True(session.StartSession >= expectedDate));
            _sessionRepository.Verify(repo => repo.GetSessionsForStatisticAsync(filter), Times.Once);
        }
    }
}
