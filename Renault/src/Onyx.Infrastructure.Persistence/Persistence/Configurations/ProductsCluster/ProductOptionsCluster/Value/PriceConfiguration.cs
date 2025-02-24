using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductOptionsCluster.Value;
public class PriceConfiguration : IEntityTypeConfiguration<Price>
{
    public void Configure(EntityTypeBuilder<Price> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Date)
            .HasComment("تاریخ قیمت")
            .IsRequired();
        builder.Property(t => t.MainPrice)
            .HasComment("قیمت اصلی")
            .IsRequired()
            .HasPrecision(18, 2);


        builder.HasOne(b => b.ProductAttributeOption)
            .WithMany(b => b.Prices);
    }
}
