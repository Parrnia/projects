using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductAttributesCluster;
public class ProductTypeAttributeGroupCustomFieldConfiguration : IEntityTypeConfiguration<ProductTypeAttributeGroupCustomField>
{
    public void Configure(EntityTypeBuilder<ProductTypeAttributeGroupCustomField> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.FieldName)
            .HasComment("نام")
            .IsRequired();
        builder.Property(t => t.Value)
            .HasComment("مقدار فیلد")
            .IsRequired();
        builder.Property(t => t.ProductTypeAttributeGroupId)
            .HasComment("گروه نوع ویژگی محصول")
            .IsRequired();
        

        builder.HasOne(b => b.ProductTypeAttributeGroup)
            .WithMany(b => b.ProductTypeAttributeGroupCustomFields);
    }
}
