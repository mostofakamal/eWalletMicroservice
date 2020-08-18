using Kyc.Domain.AggregateModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kyc.Insfrastructure.EntityConfigurations
{
    public class KycInformationConfiguration : IEntityTypeConfiguration<KycInformation>
    {
        public void Configure(EntityTypeBuilder<KycInformation> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.KycStatus).WithMany().HasForeignKey(x => x.KycStatusId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(x=> x.CreatedTime);
        }
    }
}
