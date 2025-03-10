using Microsoft.EntityFrameworkCore;
using Tracker.DatabaseCatalog;
using Tracker.DatabaseCatalog.Configurations;
using Tracker.Entitites;

namespace Tracker.Repositories
{
    public class TrackerDbContext : DbContext
    {
        public TrackerDbContext(DbContextOptions<TrackerDbContext> options)
        : base(options) { }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Goal> Goals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SessionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new  GoalEntityConfiguration());

            DbCategorySeeder.SeedCategories(modelBuilder);
        }
    }
}
