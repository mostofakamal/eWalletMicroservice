using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transaction.Domain.AggregateModel;

namespace Transaction.Infrastructure.EntityConfigurations
{
    public class PendingTransactionEntityTypeConfiguration : IEntityTypeConfiguration<PendingTransaction>
    {
        public void Configure(EntityTypeBuilder<PendingTransaction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Ignore(b => b.DomainEvents);
            builder.Property(x => x.ScheduledOn).HasDefaultValue(DateTime.UtcNow);
            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.CorrelationId).IsRequired(false);
            builder.Property(x => x.HandledOn).IsRequired(false);
            builder
                .Property<int>("_senderUserId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("SenderUserId")
                .IsRequired();

            builder
                .Property<int>("_receiverUserId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ReceiverUserId")
                .IsRequired();

            builder.HasOne(o => o.SenderUser)
                .WithMany().HasForeignKey("_senderUserId")
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(o => o.ReceiverUser)
                .WithMany()
                .HasForeignKey("_receiverUserId")
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property<int>("_transactionTypeId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("TransactionTypeId")
                .IsRequired();

            builder.HasOne(o => o.TransactionType)
                .WithMany().HasForeignKey("_transactionTypeId");

        }
    }
}