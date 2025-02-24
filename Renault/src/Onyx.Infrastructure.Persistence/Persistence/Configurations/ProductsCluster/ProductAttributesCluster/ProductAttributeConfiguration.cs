using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductAttributesCluster;
public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
{
    public void Configure(EntityTypeBuilder<ProductAttribute> builder)
    {
        builder.HasIndex(pa => new { pa.Name, pa.ProductId })
            .IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .HasComment("نام")
            .IsRequired();
        builder.Property(t => t.Slug)
            .HasComment("عنوان کوتاه")
            .IsRequired();
        builder.Property(t => t.ValueName)
            .HasComment("نام مقدار")
            .IsRequired();
        builder.Property(t => t.ValueSlug)
            .HasComment("عنوان کوتاه مقدار")
            .IsRequired();
        builder.Property(t => t.Featured)
            .HasComment("ویژه")
            .IsRequired();
        builder.Property(t => t.ProductId)
            .HasComment("محصول مرتبط")
            .IsRequired();
        

        builder.HasMany(b => b.ProductAttributeValueCustomFields)
            .WithOne(f => f.ProductAttribute);
        builder.HasMany(b => b.ProductAttributeCustomFields)
            .WithOne(f => f.ProductAttribute);
        builder.HasOne(b => b.Product)
            .WithMany(b => b.Attributes);
    }
}
