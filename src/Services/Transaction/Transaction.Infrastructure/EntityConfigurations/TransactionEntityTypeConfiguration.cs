using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Transaction.Infrastructure.EntityConfigurations
{
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Domain.AggregateModel.Transaction>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregateModel.Transaction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Ignore(b => b.DomainEvents);
            builder.Property(x => x.TransactionGuid).IsRequired();
            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.CounterPartyUserGuid).IsRequired();
            builder
                .Property<int>("_transactionTypeId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("TransactionTypeId")
                .IsRequired();
            builder
                .Property<int>("_transactionStatusId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("TransactionStatusId")
                .IsRequired();

            builder.HasOne(o => o.TransactionType)
                .WithMany().HasForeignKey("_transactionTypeId");

            builder.HasOne(o => o.TransactionStatus)
                .WithMany().HasForeignKey("_transactionStatusId");
        }
    }
}