using Kyc.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kyc.Insfrastructure.EntityConfigurations
{
    public class KycStatusConfiguration : IEntityTypeConfiguration<KycStatus>
    {
        public void Configure(EntityTypeBuilder<KycStatus> builder)
        {
            builder.HasData(new List<KycStatus>()
            {
                new KycStatus()
                {
                    Id = 1,
                    Name = "Approved"
                },
                new KycStatus()
                {
                    Id = 2,
                    Name = "Pending"
                },
                new KycStatus()
                {
                    Id = 3,
                    Name = "Failed"
                }
            });
        }
    }
}
