using Microsoft.EntityFrameworkCore;
using Tracker.Entitites;
using Tracker.Entitites.Enums;
using Tracker.Interfaces.RepositoryInterfaces;
using Tracker.Repositories;

namespace Tracker.DatabaseCatalog.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TrackerDbContext _dbContext;

        public CategoryRepository(TrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category> CreateCategoryAsync(string name)
        {
            var createdCategory = await _dbContext.Categories.AddAsync(new Category { Name = name });

            return createdCategory.Entity;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if(category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = await _dbContext.Categories.ToListAsync();

            return categories;
        }

        public async Task<Category> RenameCategoryAsync(string name, int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return null;
            }

            category.Name = name;
            await _dbContext.SaveChangesAsync();

            return category;
        }

    }
}
