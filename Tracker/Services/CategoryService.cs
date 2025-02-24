using Tracker.Entitites;
using Tracker.Entitites.Enums;
using Tracker.Interfaces;

namespace Tracker.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> CreateCategoryAsync(string name)
        {
            var createdCategory = await _categoryRepository.CreateCategoryAsync(name);

            return createdCategory;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepository.DeleteCategoryAsync(id);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            return categories;
        }

        public async Task<Category> RenameCategoryAsync(string name, int id)
        {
            var renamedCategory = await _categoryRepository.RenameCategoryAsync(name, id);

            return renamedCategory;
        }
    }
}
