﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reward.Domain.AggregateModel;

namespace Reward.Infrastructure.EntityConfigurations
{
    public class UserRewardStatusConfiguration : IEntityTypeConfiguration<UserRewardStatus>
    {
        public void Configure(EntityTypeBuilder<UserRewardStatus> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasData(UserRewardStatus.List());
        }
    }
}