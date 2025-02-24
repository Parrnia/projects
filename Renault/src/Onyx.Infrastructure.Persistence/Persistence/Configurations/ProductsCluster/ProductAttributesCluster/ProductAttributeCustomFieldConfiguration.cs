using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductAttributesCluster;
public class ProductAttributeCustomFieldConfiguration : IEntityTypeConfiguration<ProductAttributeCustomField>
{
    public void Configure(EntityTypeBuilder<ProductAttributeCustomField> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.FieldName)
            .HasComment("نام")
            .IsRequired();
        builder.Property(t => t.Value)
            .HasComment("مقدار فیلد")
            .IsRequired();
        builder.Property(t => t.ProductAttributeId)
            .HasComment("ویژگی محصول")
            .IsRequired();


        builder.HasOne(b => b.ProductAttribute)
            .WithMany(b => b.ProductAttributeCustomFields);
    }
}
