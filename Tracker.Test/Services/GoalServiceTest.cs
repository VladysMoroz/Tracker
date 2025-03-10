using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Tracker.Entitites;
using Tracker.Interfaces.RepositoryInterfaces;
using Tracker.Services;
using Xunit;

namespace Tracker.Test.Services
{
    public class GoalServiceTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IGoalRepository> _repositoryMock;
        private readonly GoalService _sut;

        public GoalServiceTest()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
            _repositoryMock = _fixture.Freeze<Mock<IGoalRepository>>();
            _sut = _fixture.Build<GoalService>().OmitAutoProperties().Create();
        }

        [Fact]
        public async Task CreateGoalAsync_ReturnsCreatedGoal()
        {
            // Arrange 
            //var goal = new Goal() { Name = "Test", Id = 1 };
            var goal = _fixture.Build<Goal>().Without(x => x.Category).Create();


            _repositoryMock
                .Setup(x => x.CreateGoalAsync(goal))
                .ReturnsAsync(goal);

            // Act
            var result = await _sut.CreateGoalAsync(goal);

            // Assert
            Assert.IsType<Goal>(result);
            Assert.NotNull(result);

            _repositoryMock.Verify(s => s.CreateGoalAsync(goal), Times.Once);
        }

        [Fact]
        public async Task EditGoalAsync_ReturnsEditedGoal()
        {
            // Arrange
            var editedGoal = _fixture.Build<Goal>().Without(x => x.Category).Create();

            _repositoryMock
                .Setup(x => x.EditGoalAsync(editedGoal))
                .ReturnsAsync(editedGoal);

            // Act
            var result = await _sut.EditGoalAsync(editedGoal);

            // Assert
            Assert.IsType<Goal>(result);
            Assert.NotNull(result);
        }
    }
}
