using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Runtime.CompilerServices;
using Tracker.Controllers;
using Tracker.Entitites;
using Tracker.Entitites.Filters;
using Tracker.Interfaces.ServiceInterfaces;
using Tracker.Repositories;
using Tracker.Services;
using Xunit;

namespace Tracker.Test.Controllers
{
    public class SessionControllerTest
    {
        private readonly SessionController _sut;
        private readonly Mock<ISessionService> _sessionService;

        public SessionControllerTest()
        {
            _sessionService = new Mock<ISessionService>();
            _sut = new SessionController(_sessionService.Object);
        }

        [Fact]
        public async Task CreateSessionAsync_CorrectCategoryName_CreateNewSession()
        {
            // Arrange
            var session = new Session() { StartSession = DateTime.Now };

            var categoryName = "Test";

            _sessionService
                    .Setup(x => x.CreateSessionAsync(It.IsAny<string>()))
                    .ReturnsAsync(session);

            // Act
            var createdSession = await _sut.CreateSessionAsync(categoryName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(createdSession);
            var sess = Assert.IsType<Session>(okResult.Value);
            Assert.NotNull(session);
        }

        [Fact]
        public async Task GetSessionsForStatisticAsync_ValidFitler_ReturnsOkWithSession()
        {
            // Arrange
            Filter nullableFitler = null;
            var filter = new Filter
            {
                Option = Entitites.Enums.OptionsForDisplayingStats.CurrentDay,
            };
            var expectedSessions = new List<Session> { new Session { StartSession = DateTime.Now } };
            
            _sessionService.Setup(service => service.GetSessionsForStatisticAsync(filter))
                .ReturnsAsync(expectedSessions);

            // Act
            var result = await _sut.GetSessionsForStatisticAsync(filter);
            var resultOfDropNullableFilter = await _sut.GetSessionsForStatisticAsync(nullableFitler);

            // Assert

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualSessions = Assert.IsType<List<Session>>(okResult.Value);
            Assert.Equal(expectedSessions, actualSessions);

            var okResult2 = Assert.IsType<BadRequestObjectResult>(resultOfDropNullableFilter);
        }


        // The signature of the unit-test method:
        // MethodName_StateUnderTest_ExpectedBehavior
        /*MethodName – назва методу, який тестується (наприклад, GetSessionsForStatisticAsync).
        StateUnderTest – умова або вхідні дані (ReturnsNull, ReturnsEmptyList).
        ExpectedBehavior – очікуваний результат (ReturnsNotFound, ReturnsOkWithEmptyList). */
        [Fact]
        public async Task GetSessionsForStatisticAsync_WhenServiceReturnsEmptyList_ReturnsOkWithEmptyList()
        {
            // Arrange
            var filter = new Filter
            {
                Option = Entitites.Enums.OptionsForDisplayingStats.Day,
                Quantity = 5
            };
            var sessions = new List<Session>();

            _sessionService.Setup(service => service.GetSessionsForStatisticAsync(filter))
                           .ReturnsAsync(sessions);
            // Act
            var result = await _sut.GetSessionsForStatisticAsync(filter);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var unboxedResult = Assert.IsType<List<Session>>(okObjectResult.Value);
            Assert.NotNull(sessions);
            Assert.Empty(sessions);
        }
    }
}
        
