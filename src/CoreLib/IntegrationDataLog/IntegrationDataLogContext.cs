using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationDataLog
{
    public class IntegrationDataLogContext : DbContext
    {
        public IntegrationDataLogContext(DbContextOptions<IntegrationDataLogContext> options) : base(options)
        {
        }

        public DbSet<IntegrationDataLogEntry> IntegrationDataLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IntegrationDataLogEntry>(ConfigureIntegrationDataLogEntry);
        }

        void ConfigureIntegrationDataLogEntry(EntityTypeBuilder<IntegrationDataLogEntry> builder)
        {
            builder.HasKey(e => e.IntegrationDataId);

            builder.Property(e => e.IntegrationDataId)
                .IsRequired();

            builder.Property(e => e.Content)
                .IsRequired();

            builder.Property(e => e.CreationTime)
                .IsRequired();

            builder.Property(e => e.State)
                .IsRequired();
            builder.Property(e => e.IntegrationDataType)
                .IsRequired();

            builder.Property(e => e.TimesSent)
                .IsRequired();

            builder.Property(e => e.DataTypeName)
                .IsRequired();

        }
    }
}
