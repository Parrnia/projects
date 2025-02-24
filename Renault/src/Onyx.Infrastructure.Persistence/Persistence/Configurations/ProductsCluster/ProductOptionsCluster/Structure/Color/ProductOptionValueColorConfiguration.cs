using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductOptionsCluster.Structure.Color;
public class ProductOptionValueColorConfiguration : IEntityTypeConfiguration<ProductOptionValueColor>
{
    public void Configure(EntityTypeBuilder<ProductOptionValueColor> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .HasComment("نام")
            .IsRequired();
        builder.Property(t => t.Slug)
            .HasComment("عنوان کوتاه")
            .IsRequired();
        builder.Property(t => t.Color)
            .HasComment("رنگ")
            .IsRequired();
        builder.Property(t => t.ProductOptionColorId)
            .HasComment("ویژگی رنگ محصول")
            .IsRequired();


        builder.HasOne(b => b.ProductOptionColor)
            .WithMany(b => b.Values);
    }
}
