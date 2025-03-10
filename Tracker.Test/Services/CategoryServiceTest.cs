using Moq;
using Tracker.Entitites;
using Tracker.Interfaces.RepositoryInterfaces;
using Tracker.Services;
using Xunit;

namespace Tracker.Test.Services
{
    public class CategoryServiceTest
    {
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly CategoryService _sut; // System under Test; та система, що тестується
        public CategoryServiceTest()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
            _sut = new CategoryService(_categoryRepository.Object);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldReturnCreatedCategory()
        {
            // Arrange
            var categoryName = "New Category";
            var expectedCategory = new Category { Id = 1, Name = categoryName };

            _categoryRepository
                .Setup(repo => repo.CreateCategoryAsync(categoryName))
                .ReturnsAsync(expectedCategory);

            // Act
            var result = await _sut.CreateCategoryAsync(categoryName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCategory.Id, result.Id);
            Assert.Equal(expectedCategory.Name, result.Name);
            _categoryRepository.Verify(repo => repo.CreateCategoryAsync(categoryName), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldCallRepositoryMethodOnce()
        {
            // Arrange
            var categoryId = 1;

            _categoryRepository
                .Setup(repo => repo.DeleteCategoryAsync(categoryId))
                .Returns(Task.CompletedTask);

            // Act
            await _sut.DeleteCategoryAsync(categoryId);

            // Assert
            _categoryRepository.Verify(repo => repo.DeleteCategoryAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnListOfCategories()
        {
            // Arrange
            var expectedCategories = new List<Category>
        {
            new Category { Id = 1, Name = "Category 1" },
            new Category { Id = 2, Name = "Category 2" }
        };

            _categoryRepository
                .Setup(repo => repo.GetAllCategoriesAsync())
                .ReturnsAsync(expectedCategories);

            // Act
            var result = await _sut.GetAllCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _categoryRepository.Verify(repo => repo.GetAllCategoriesAsync(), Times.Once);
        }

        [Fact]
        public async Task RenameCategoryAsync_ShouldReturnRenamedCategory()
        {
            // Arrange
            var categoryId = 1;
            var newName = "Updated Category";
            var expectedCategory = new Category { Id = categoryId, Name = newName };

            _categoryRepository
                .Setup(repo => repo.RenameCategoryAsync(newName, categoryId))
                .ReturnsAsync(expectedCategory);

            // Act
            var result = await _sut.RenameCategoryAsync(newName, categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
            Assert.Equal(newName, result.Name);
            _categoryRepository.Verify(repo => repo.RenameCategoryAsync(newName, categoryId), Times.Once);
        }
    }
}
