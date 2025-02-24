using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Tracker.Controllers;
using Tracker.Entitites;
using Tracker.Entitites.Filters;
using Tracker.Interfaces;
using Xunit;

namespace Tracker.Test.Controllers
{
    public class CategoryServiceTest
    {
        private readonly Mock<ICategoryService> _categoryService;
        private readonly CategoryController _sut; // System under Test; та система, над якою проводять тести
        public CategoryServiceTest()
        {
            _categoryService = new Mock<ICategoryService>();
            _sut = new CategoryController(_categoryService.Object);
        }

        [Fact]
        public async Task CreateCategoryAsync_InputParameterIsNull_ThrowArgumentNullException()
        {
            // Arrange
            string name = null;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _sut.CreateCategoryAsync(name));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_WhenCalled_ReturnsAllCategoriesWithOkStatus()
        {
            // Arrange
            var categories = new List<Category> 
            { 
                new Category
                {
                    Id = 1, Name = "Test1"
                },
                new Category
                {
                    Id = 2, Name ="Test2"
                } 
            };
            _categoryService.Setup(service => service.GetAllCategoriesAsync())
                            .ReturnsAsync(categories);

            // Act
            var result = await _sut.GetAllCategoriesAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategories = Assert.IsType<List<Category>>(okResult.Value);
            Assert.NotEmpty(returnedCategories);
            Assert.Equal(categories.Count, returnedCategories.Count);
            Assert.Equal(categories, returnedCategories);
        }

        [Fact]
        public async Task RenameCategoryAsync_WhenCalled_ReturnsUpdatedCategoryWithOkResult()
        {
            // Arrange
            var newName = "Programming";
            int id = 1;

            var updatedCategory = new Category { Id = id, Name = newName };

            _categoryService.Setup(service => service.RenameCategoryAsync(newName, id))
                            .ReturnsAsync(updatedCategory);

            // Act
            var result = await _sut.RenameCategoryAsync(newName, id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategory = Assert.IsType<Category>(okResult.Value);
            Assert.Equal(newName, returnedCategory.Name);
        }

        [Fact]
        public async Task RenameCategoryAsync_WhenNewNameIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            string nullableNewName = null;
            int id = 2;

            _categoryService.Setup(service => service.RenameCategoryAsync(nullableNewName, id))
                            .ThrowsAsync(new ArgumentNullException(nameof(nullableNewName)));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _sut.RenameCategoryAsync(nullableNewName, id));

            Assert.Equal("new Name cannot be null or empty (Parameter 'newName')", exception.Message);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldReturnOk_WhenDeletionIsSuccessful()
        {
            // Arrange
            int categoryId = 1;
            _categoryService.Setup(s => s.DeleteCategoryAsync(categoryId)).Returns(Task.CompletedTask);

            // Act
            var result = await _sut.DeleteCategoryAsync(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Category has been deleted successfully!", okResult.Value);
            _categoryService.Verify(s => s.DeleteCategoryAsync(categoryId), Times.Once);
        }

        //[Fact]
        //public async Task DeleteCategoryAsync_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        //{
        //    // Arrange
        //    int invalidCategoryId = 99;
        //    _categoryService.Setup(s => s.DeleteCategoryAsync(invalidCategoryId))
        //                    .ThrowsAsync(new KeyNotFoundException("Category not found"));

        //    // Act
        //    var result = await _sut.DeleteCategoryAsync(invalidCategoryId);

        //    // Assert
        //    var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        //    Assert.Equal("Category not found", notFoundResult.Value);
        //    _categoryService.Verify(s => s.DeleteCategoryAsync(invalidCategoryId), Times.Once);
        //}
    }
}
