using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reward.Domain.AggregateModel;

namespace Reward.Infrastructure.EntityConfigurations
{
    public class RewardRuleConfiguration : IEntityTypeConfiguration<RewardRule>
    {
        public void Configure(EntityTypeBuilder<RewardRule> builder)
        {
            builder.Property(x => x.Amount).IsRequired().HasColumnType($"decimal(18,2)");
        }
    }
}
