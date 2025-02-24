using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductAttributesCluster;
public class ProductTypeAttributeGroupConfiguration : IEntityTypeConfiguration<ProductTypeAttributeGroup>
{
    public void Configure(EntityTypeBuilder<ProductTypeAttributeGroup> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .HasComment("نام")
            .IsRequired();
        builder.Property(t => t.Slug)
            .HasComment("عنوان کوتاه")
            .IsRequired();


        builder.HasMany(b => b.ProductAttributeTypes)
            .WithMany(f => f.AttributeGroups);
        builder.HasMany(p => p.ProductTypeAttributeGroupCustomFields)
            .WithOne(p => p.ProductTypeAttributeGroup);
        builder.HasMany(p => p.Attributes)
            .WithOne(p => p.ProductTypeAttributeGroup);
    }
}
