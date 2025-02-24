using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster;
public class ProductCustomFieldConfiguration : IEntityTypeConfiguration<ProductCustomField>
{
    public void Configure(EntityTypeBuilder<ProductCustomField> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.FieldName)
            .HasComment("نام")
            .IsRequired();
        builder.Property(t => t.Value)
            .HasComment("مقدار فیلد")
            .IsRequired();
        builder.Property(t => t.ProductId)
            .HasComment("محصول مرتبط")
            .IsRequired();


        builder.HasOne(b => b.Product)
            .WithMany(b => b.ProductCustomFields);
    }
}
