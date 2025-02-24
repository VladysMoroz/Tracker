using Microsoft.EntityFrameworkCore;
using Tracker.Entitites;

namespace Tracker.DatabaseCatalog
{
    public static class DbCategorySeeder
    {
        public static void SeedWeapon(ModelBuilder modelBuilder)
        {
            var category = new List<Category>
            {
                new Category { Id = 1, Name = "Programming" }, 
                new Category { Id = 2, Name = "English" }
            };
            modelBuilder.Entity<Category>().HasData(category);
        }
    }
}
