using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster;
public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Image)
            .HasComment("فایل")
            .IsRequired();
        builder.Property(t => t.Order)
            .HasComment("ترتیب")
            .IsRequired();
        builder.Property(t => t.ProductId)
            .HasComment("محصول مرتبط")
            .IsRequired();

        builder.HasOne(b => b.Product)
            .WithMany(b => b.Images);
    }
}
