using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductAttributesCluster;
public class ProductTypeAttributeGroupAttributeConfiguration : IEntityTypeConfiguration<ProductTypeAttributeGroupAttribute>
{
    public void Configure(EntityTypeBuilder<ProductTypeAttributeGroupAttribute> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Value)
            .HasComment("مقدار")
            .IsRequired();
        builder.Property(t => t.ProductTypeAttributeGroupId)
            .HasComment("گروه بندی نوع ویژگی محصول")
            .IsRequired();
        

        builder.HasOne(b => b.ProductTypeAttributeGroup)
            .WithMany(b => b.Attributes);
    }
}
