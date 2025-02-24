using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ReturnOrdersCluster;
public class ReturnOrderConfiguration : IEntityTypeConfiguration<ReturnOrder>
{
    public void Configure(EntityTypeBuilder<ReturnOrder> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Token)
            .HasComment("رمز مبادله با سون")
            .IsRequired();
        builder.Property(t => t.Number)
            .HasComment("شماره")
            .IsRequired();
        builder.Property(t => t.Quantity)
            .HasComment("تعداد")
            .IsRequired();
        builder.Property(t => t.Subtotal)
            .HasComment("جمع قیمت کل محصولات")
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(t => t.Total)
            .HasComment("مبلغ پرداختی")
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(t => t.CreatedAt)
            .HasComment("زمان ثبت بازگشت سفارش")
            .IsRequired();
        builder.Property(t => t.CostRefundType)
            .HasComment("شیوه بازپرداخت")
            .IsRequired();
        builder.Property(t => t.ReturnOrderTransportationType)
            .HasComment("شیوه بازگشت کالا")
            .IsRequired();
        builder.Property(t => t.CustomerAccountInfo)
            .HasComment("اطلاعات حساب");
        builder.Property(t => t.OrderId)
            .HasComment("سفارش مرتبط")
            .IsRequired();

        builder.HasMany(b => b.ReturnOrderStateHistory)
            .WithOne(f => f.ReturnOrder);
        builder.HasMany(b => b.ItemGroups)
            .WithOne(f => f.ReturnOrder);
        builder.HasMany(b => b.Totals)
            .WithOne(f => f.ReturnOrder);

        builder.HasOne(b => b.Order)
            .WithMany(f => f.ReturnOrders);
    }
}
