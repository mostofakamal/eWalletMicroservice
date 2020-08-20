using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reward.Domain.AggregateModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reward.Infrastructure.EntityConfigurations
{
    public class RewardOperationConfiguration : IEntityTypeConfiguration<RewardOperation>
    {
        public void Configure(EntityTypeBuilder<RewardOperation> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(20);
            builder.HasData(new List<RewardOperation>
            {
                new RewardOperation()
                {
                    Id = 1,
                    Name = "SubmitKyc"
                },
                new RewardOperation
                {
                    Id = 2,
                    Name = "TransferMoney"
                },
                new RewardOperation
                {
                    Id = 3,
                    Name = "BillPayment"
                }
            });
        }
    }
}
