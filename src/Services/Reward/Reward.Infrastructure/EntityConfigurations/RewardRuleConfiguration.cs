using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reward.Domain.AggregateModel;
using System;

namespace Reward.Infrastructure.EntityConfigurations
{
    public class RewardRuleConfiguration : IEntityTypeConfiguration<RewardRule>
    {
        public void Configure(EntityTypeBuilder<RewardRule> builder)
        {
            builder.Property(x => x.Amount).IsRequired().HasColumnType($"decimal(18,2)");

            builder.HasData(
                new RewardRule[]
                {
                     new RewardRule
                    {
                        Id = 1,
                        Amount = 2,
                        IsEnabled = true,
                        OperationId = (short) RewardOperation.SubmitKyc.Id,
                        RequiredMinOccurance = 1,
                        ValidFrom = DateTime.UtcNow,
                        ValidTo = DateTime.UtcNow.AddMonths(1)
                    },

                    new RewardRule
                    {
                        Id = 2,
                        Amount = 1,
                        IsEnabled = true,
                        OperationId = (short)RewardOperation.TransferMoney.Id,
                        RequiredMinOccurance = 2,
                        ValidFrom = DateTime.UtcNow,
                        ValidTo = DateTime.UtcNow.AddMonths(2)
                    },

                    new RewardRule
                    {
                        Id = 3,
                        Amount = 2,
                        IsEnabled = true,
                        OperationId = (short)RewardOperation.BillPayment.Id,
                        RequiredMinOccurance = 1,
                        ValidFrom = DateTime.UtcNow,
                        ValidTo = DateTime.UtcNow.AddMonths(2)
                    }
                });
        }
    }
}
