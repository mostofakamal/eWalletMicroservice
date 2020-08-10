using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Models
{
    public class Country
    {
        public int Id { get; set; }

        public Guid CountryGuid { get; set; }

        public string Name { get; set; }

        public string IsoCode { get; set; }

        public string CurrencyCode { get; set; }
    }

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