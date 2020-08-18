using Kyc.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Kyc.Insfrastructure.EntityConfigurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            // Seed Country

            builder.HasData(new List<Country>()
            {
                new Country()
                {
                    Id = 1,
                    Name = "Bangladesh",
                    IsoCode = "BD",
                    CurrencyCode = "BDT",
                    CountryGuid = Guid.NewGuid()
                },
                new Country()
                {
                    Id = 2,
                    Name = "India",
                    IsoCode = "IN",
                    CurrencyCode = "INR",
                    CountryGuid = Guid.NewGuid()
                },
                new Country()
                {
                    Id = 3,
                    Name = "Norway",
                    IsoCode = "NO",
                    CurrencyCode = "NOK",
                    CountryGuid = Guid.NewGuid()
                }
            });
        }
    }
}
