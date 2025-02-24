using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductOptionsCluster.Value;
public class BadgeConfiguration : IEntityTypeConfiguration<Badge>
{
    public void Configure(EntityTypeBuilder<Badge> builder)
    {
        builder.HasIndex(t => t.Value).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Value)
            .HasComment("مقدار")
            .IsRequired();


        builder.HasMany(b => b.ProductAttributeOptions)
            .WithMany(f => f.Badges);

    }
}
