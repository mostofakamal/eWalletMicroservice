using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reward.Domain.AggregateModel;

namespace Reward.Infrastructure.EntityConfigurations
{

    public class UserRewardConfiguration : IEntityTypeConfiguration<UserReward>
    {
        public void Configure(EntityTypeBuilder<UserReward> builder)
        {
            builder.HasOne(_ => _.User).WithMany(r => r.UserRewards)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            
            builder.Property(x => x.RewardGuid).IsRequired();
            
            builder
                .Property<int>("_statusId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("StatusId")
                .IsRequired();

            builder.HasOne(x => x.Status).WithMany().HasForeignKey("_statusId");
        }
    }
}
