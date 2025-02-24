using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductOptionsCluster.Structure.Material;
public class ProductOptionMaterialCustomFieldConfiguration : IEntityTypeConfiguration<ProductOptionMaterialCustomField>
{
    public void Configure(EntityTypeBuilder<ProductOptionMaterialCustomField> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.FieldName)
            .HasComment("نام")
            .IsRequired();
        builder.Property(t => t.Value)
            .HasComment("مقدار فیلد")
            .IsRequired();
        builder.Property(t => t.ProductOptionMaterialId)
            .HasComment("مقدار ویژگی جنس محصول")
            .IsRequired();


        builder.HasOne(b => b.ProductOptionMaterial)
            .WithMany(b => b.ProductOptionMaterialCustomFields);
    }
}
