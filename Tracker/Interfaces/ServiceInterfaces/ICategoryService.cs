using Tracker.Entitites;
using Tracker.Entitites.Enums;

namespace Tracker.Interfaces.ServiceInterfaces
{
    public interface ICategoryService
    {
        Task<Category> CreateCategoryAsync(string name);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> RenameCategoryAsync(string name, int id);
        Task DeleteCategoryAsync(int id);
    }
}
