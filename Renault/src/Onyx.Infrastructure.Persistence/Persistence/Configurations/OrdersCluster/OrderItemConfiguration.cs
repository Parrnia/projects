using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.OrdersCluster;
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Price)
            .HasComment("قیمت واحد")
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(t => t.DiscountPercentForProduct)
            .HasComment("درصد تخفیف محاسبه شده بر روی کالا")
            .HasPrecision(5, 2)
            .IsRequired();
        builder.Property(t => t.Quantity)
            .HasComment("تعداد")
            .HasPrecision(18, 3)
            .IsRequired();
        builder.Property(t => t.Total)
            .HasComment("جمع کل قیمت سفارش")
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(t => t.OrderId)
            .HasComment("سفارش مرتبط")
            .IsRequired();
        builder.Property(t => t.ProductLocalizedName)
            .HasComment("نام فارسی قطعه")
            .IsRequired();
        builder.Property(t => t.ProductName)
            .HasComment("نام لاتین قطعه")
            .IsRequired();
        builder.Property(t => t.ProductNo)
            .HasComment("کد کالا");
        builder.Property(t => t.ProductAttributeOptionId)
            .IsRequired();


        builder.HasOne(b => b.ProductAttributeOption)
            .WithMany(b => b.OrderItems);
        builder.HasOne(b => b.Order)
            .WithMany(b => b.Items);
        builder.HasMany(b => b.OptionValues)
            .WithOne(e => e.OrderItem);
        builder.HasMany(b => b.Options)
            .WithOne(f => f.OrderItem);
    }
}
