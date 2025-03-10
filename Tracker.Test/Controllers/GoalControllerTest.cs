using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tracker.Controllers;
using Tracker.Entitites;
using Tracker.Entitites.ViewModels;
using Tracker.Interfaces.ServiceInterfaces;
using Xunit;

namespace Tracker.Test.Controllers
{
    public class GoalControllerTest
    {
        private readonly IFixture _fixture;
        private readonly GoalController _sut;
        private readonly Mock<IGoalService> _goalService;
        private readonly Mock<IMapper> _mapper;

        public GoalControllerTest()
        {
            _goalService = new Mock<IGoalService>();
            _mapper = new Mock<IMapper>();
            _sut = new GoalController(_goalService.Object,_mapper.Object);
        }

        [Fact]
        public async Task GetAllGoalsAsync_WhenGoalsAreNull_ReturnsBadRequest()
        {
            // Arrange
            _goalService
                .Setup(x => x.GetAllGoalsAsync())
                .ReturnsAsync((List<Goal>)null);

            // Act
            var result = await _sut.GetAllGoalsAsync();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Method has returned nullable value", badRequestResult?.Value);
        }

        [Fact]
        public async Task GetAllGoalsAsync_WhenGoalsContainsValues_ReturnsOkObjectResult()
        {
            // arrange
            var goals = new List<Goal> { new Goal { Id = 1 }, new Goal { Id = 2} };

            _goalService.Setup(x => x.GetAllGoalsAsync())
                        .ReturnsAsync(goals);

            // act
            var result = await _sut.GetAllGoalsAsync();

            // assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var unboxedResult = Assert.IsType<List<Goal>>(okObjectResult.Value);
            Assert.NotNull(unboxedResult);
            Assert.NotEmpty(unboxedResult);
        }

        [Fact]
        public async Task CreateGoalAsync_WhenGoalsIsNotNull_ReturnsOkObjectResult()
        {
            // arrange
            var goalViewModel = new GoalForCreatingViewModel { Name = "Test", CategoryId = 1 };
            var goal = new Goal { CategoryId = 1, Name = "Test" };

            _mapper.Setup(x => x.Map<Goal>(It.IsAny<GoalForCreatingViewModel>())).Returns(goal);
            _goalService.Setup(x => x.CreateGoalAsync(goal)).ReturnsAsync((Goal)null);

            // act
            var result = await _sut.CreateGoalAsync(goalViewModel);

            // assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Goal has not been successfully created", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateGoalAsync_WhenCreatedGoalIsCorrect_ReturnsOkObjectResult()
        {
            // arrange
            var goalViewModel = new GoalForCreatingViewModel { Name = "Test", CategoryId = 1 };
            var goal = new Goal { CategoryId = 1, Name = "Test" };

            _mapper.Setup(x => x.Map<Goal>(It.IsAny<GoalForCreatingViewModel>())).Returns(goal);
            _goalService.Setup(x => x.CreateGoalAsync(goal)).ReturnsAsync(goal);

            // act
            var result = await _sut.CreateGoalAsync(goalViewModel);

            // assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var unboxedResult = Assert.IsType<Goal>(okObjectResult.Value);
            Assert.NotNull(unboxedResult);
        }
    }
}
