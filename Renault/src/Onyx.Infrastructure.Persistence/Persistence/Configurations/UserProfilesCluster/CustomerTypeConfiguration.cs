using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.UserProfilesCluster;
public class CustomerTypeConfiguration : IEntityTypeConfiguration<CustomerType>
{
    public void Configure(EntityTypeBuilder<CustomerType> builder)
    {
        builder.HasIndex(t => t.CustomerTypeEnum).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.DiscountPercent)
            .HasComment("درصد تخفیف")
            .HasPrecision(5, 2)
            .IsRequired();
    }
}