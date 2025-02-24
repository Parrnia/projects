using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductOptionsCluster.Value;
public class ProductAttributeOptionConfiguration : IEntityTypeConfiguration<ProductAttributeOption>
{
    public void Configure(EntityTypeBuilder<ProductAttributeOption> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.TotalCount)
            .HasComment("تعداد")
            .HasPrecision(18, 3)
            .IsRequired();
        builder.Property(t => t.SafetyStockQty)
            .HasComment("مقدار ذخیره احتیاطی")
            .HasPrecision(18, 3)
            .IsRequired();
        builder.Property(t => t.MinStockQty)
            .HasComment("مقدار حداقل موجودی")
            .HasPrecision(18, 3)
            .IsRequired();
        builder.Property(t => t.MaxStockQty)
            .HasComment("مقدار حداکثر موجودی")
            .HasPrecision(18, 3)
            .IsRequired();
        builder.Property(t => t.ProductId)
            .IsRequired();
        builder.Property(t => t.IsDefault)
            .IsRequired();
        builder.Property(t => t.MaxSalePriceNonCompanyProductPercent)
            .HasComment("سقف قیمت فروش کالای غیر شرکتی-درصد")
            .HasPrecision(5, 2);


        builder.HasMany(x => x.Prices)
            .WithOne(x => x.ProductAttributeOption);
        builder.HasMany(x => x.OrderItems)
            .WithOne(x => x.ProductAttributeOption);
        builder.HasMany(x => x.OptionValues)
            .WithOne(x => x.ProductAttributeOption);
        builder.HasMany(p => p.ProductAttributeOptionRoles)
            .WithOne(t => t.ProductAttributeOption);

        builder.HasMany(p => p.Badges)
            .WithMany(t => t.ProductAttributeOptions);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.AttributeOptions);

        
    }
}
