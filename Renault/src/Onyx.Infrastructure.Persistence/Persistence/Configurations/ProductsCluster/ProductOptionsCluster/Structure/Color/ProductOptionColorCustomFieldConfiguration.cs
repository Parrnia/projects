using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductOptionsCluster.Structure.Color;
public class ProductOptionColorCustomFieldConfiguration : IEntityTypeConfiguration<ProductOptionColorCustomField>
{
    public void Configure(EntityTypeBuilder<ProductOptionColorCustomField> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.FieldName)
            .HasComment("نام")
            .IsRequired();
        builder.Property(t => t.Value)
            .HasComment("مقدار فیلد")
            .IsRequired();
        builder.Property(t => t.ProductOptionColorId)
            .HasComment("مقدار ویژگی رنگ محصول")
            .IsRequired();


        builder.HasOne(b => b.ProductOptionColor)
            .WithMany(b => b.ProductOptionColorCustomFields);
    }
}
