using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductOptionsCluster.Value;
public class ProductAttributeOptionValueConfiguration : IEntityTypeConfiguration<ProductAttributeOptionValue>
{
    public void Configure(EntityTypeBuilder<ProductAttributeOptionValue> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .HasComment("نام گزینه ساختاری")
            .IsRequired();
        builder.Property(t => t.Value)
            .HasComment("مقدار ویژگی محصول")
            .IsRequired();


        builder.HasOne(b => b.ProductAttributeOption)
            .WithMany(b => b.OptionValues);
    }
}
