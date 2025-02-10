using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracker.Entitites;

namespace Tracker.DatabaseCatalog.Configurations
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.Property(x => x.Name).IsUnicode(true).HasMaxLength(50).IsRequired();

            builder.HasMany(x => x.Sessions)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
