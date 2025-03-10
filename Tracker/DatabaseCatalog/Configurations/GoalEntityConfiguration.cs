using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracker.Entitites;

namespace Tracker.DatabaseCatalog.Configurations
{
    public class GoalEntityConfiguration : IEntityTypeConfiguration<Goal>
    {
        public void Configure(EntityTypeBuilder<Goal> builder)
        {
            builder.ToTable("Goals");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(256).IsUnicode(true);

            builder.Property(s => s.DeadLine).IsRequired(true).HasPrecision(0);

            builder.Property(s => s.DailyLimit).IsRequired(true).HasColumnType("time");

            builder.Property(s => s.CreatedAt).IsRequired(true).HasPrecision(0);

            builder.Property(s => s.FinishedAt).HasPrecision(0);

            builder.Property(s => s.IsFinished).HasDefaultValue(false);

            builder.HasOne(x => x.Category)
                   .WithMany(x => x.Goals)
                   .HasForeignKey(x => x.CategoryId);
        }
    }
}
