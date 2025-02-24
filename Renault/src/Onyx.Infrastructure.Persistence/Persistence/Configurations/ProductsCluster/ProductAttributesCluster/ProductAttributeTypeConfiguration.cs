using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductAttributesCluster;
public class ProductAttributeTypeConfiguration : IEntityTypeConfiguration<ProductAttributeType>
{
    public void Configure(EntityTypeBuilder<ProductAttributeType> builder)
    {
        builder.HasIndex(t => t.Name).IsUnique();


        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .HasComment("نام")
            .IsRequired();
        builder.Property(t => t.Slug)
            .HasComment("عنوان کوتاه")
            .IsRequired();
        

        builder.HasMany(b => b.AttributeGroups)
            .WithMany(f => f.ProductAttributeTypes);
        builder.HasMany(b => b.Products)
            .WithOne(f => f.ProductAttributeType);
    }
}
