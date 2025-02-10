using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracker.Entitites;

namespace Tracker.DatabaseCatalog.Configurations
{
    public class SessionEntityConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id).UseIdentityColumn(1, 1);

            builder.Property(s => s.StartSession).IsRequired(true).HasPrecision(0);

            builder.Property(s => s.EndSession).IsRequired(false).HasPrecision(0);
        }
    }
}
