using Tracker.Entitites;
using Tracker.Entitites.Enums;

namespace Tracker.Interfaces.RepositoryInterfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(string name);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> RenameCategoryAsync(string name, int id);
        Task DeleteCategoryAsync(int id);
    }
}
