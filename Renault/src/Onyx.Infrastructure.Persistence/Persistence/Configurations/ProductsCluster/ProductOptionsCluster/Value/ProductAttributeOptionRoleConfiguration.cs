using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster.ProductOptionsCluster.Value;
public class ProductAttributeOptionRoleConfiguration : IEntityTypeConfiguration<ProductAttributeOptionRole>
{
    public void Configure(EntityTypeBuilder<ProductAttributeOptionRole> builder)
    {
        builder.HasIndex(t => new { t.ProductAttributeOptionId, PersonType = t.CustomerTypeEnum}).IsUnique();


        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.MinimumStockToDisplayProductForThisCustomerTypeEnum)
            .HasComment("حداقل موجودی کالا برای نمایش کالا به کاربر")
            .HasPrecision(18, 3)
            .IsRequired();
        builder.Property(t => t.Availability)
            .HasComment("قابلیت دسترسی")
            .IsRequired();
        builder.Property(t => t.MainMaxOrderQty)
            .HasComment("حداکثر مقدار سفارش گذاری اصلی")
            .HasPrecision(18, 3)
            .IsRequired();
        builder.Property(t => t.CurrentMaxOrderQty)
            .HasComment("حداکثر مقدار سفارش گذاری کنونی")
            .HasPrecision(18, 3)
            .IsRequired();
        builder.Property(t => t.MainMinOrderQty)
            .HasComment("حداقل مقدار سفارش گذاری اصلی")
            .HasPrecision(18, 3)
            .IsRequired();
        builder.Property(t => t.CurrentMinOrderQty)
            .HasComment("حداقل مقدار سفارش گذاری کنونی")
            .HasPrecision(18, 3)
            .IsRequired();
        builder.Property(t => t.ProductAttributeOptionId)
            .IsRequired();
        builder.Property(t => t.CustomerTypeEnum)
            .IsRequired();
        builder.Property(t => t.DiscountPercent)
            .HasComment("درصد تخفیف روی کالا")
            .HasPrecision(5, 2)
            .IsRequired();


        builder.HasOne(b => b.ProductAttributeOption)
            .WithMany(b => b.ProductAttributeOptionRoles);
    }
}
