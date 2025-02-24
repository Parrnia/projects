using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.OrdersCluster.Payments;

namespace Onyx.Infrastructure.Persistence.Configurations.OrdersCluster;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasIndex(t => t.Token).IsUnique();
        builder.HasIndex(t => t.Number).IsUnique();


        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Token)
            .HasComment("رمز")
            .IsRequired();
        builder.Property(t => t.Number)
            .HasComment("شماره")
            .IsRequired();
        builder.Property(t => t.Quantity)
            .HasComment("تعداد")
            .HasPrecision(18, 3)
            .IsRequired();
        builder.Property(t => t.Subtotal)
            .HasComment("جمع قیمت کل محصولات")
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(t => t.DiscountPercentForRole)
            .HasComment("درصد تخفیف محاسبه شده بر اساس نقش")
            .HasPrecision(5, 2)
            .IsRequired();
        builder.Property(t => t.Total)
            .HasComment("مبلغ پرداختی")
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(t => t.CreatedAt)
            .HasComment("زمان سفارش")
            .IsRequired();
        builder.Property(t => t.IsPayed)
            .HasComment("پرداخت شده");


        builder.HasMany(t => t.PaymentMethods)
            .WithOne(i => i.Order)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.OrderPaymentType)
            .HasComment("شیوه پرداخت")
            .IsRequired();
        builder.Property(t => t.CustomerId)
            .HasComment("مشتری سفارش دهنده")
            .IsRequired();
        builder.Property(t => t.PhoneNumber)
            .HasComment("شماره برای ارسال پیامک")
            .IsRequired();
        builder.Property(t => t.CustomerFirstName)
            .HasComment("نام مشتری")
            .IsRequired();
        builder.Property(t => t.CustomerLastName)
            .HasComment("نام خانوادگی مشتری یا کد اقتصادی")
            .IsRequired();
        builder.Property(t => t.NationalCode)
            .HasComment("کد ملی")
            .IsRequired();
        builder.Property(t => t.PersonType)
            .HasComment("نوع هویت مشتری")
            .IsRequired();
        builder.Property(t => t.TaxPercent)
            .HasComment("درصد مالیات")
            .IsRequired();

        builder.HasOne(b => b.Customer)
            .WithMany(b => b.Orders);
        builder.OwnsOne(b => b.OrderAddress, address =>
        {
            address.Property(a => a.Title)
                .HasComment("عنوان")
                .IsRequired()
                .HasColumnName("AddressTitle");
            address.Property(a => a.Company)
                .HasComment("شرکت")
                .IsRequired()
                .HasColumnName("AddressCompany");
            address.Property(a => a.Country)
                .HasComment("کشور")
                .IsRequired()
                .HasColumnName("AddressCountry");
            address.Property(a => a.AddressDetails1)
                .HasComment("جزئیات آدرس1")
                .IsRequired()
                .HasColumnName("AddressDetails1");
            address.Property(a => a.AddressDetails2)
                .HasComment("جزئیات آدرس2")
                .HasColumnName("AddressDetails2");
            address.Property(a => a.City)
                .HasComment("شهر")
                .IsRequired()
                .HasColumnName("AddressCity");
            address.Property(a => a.State)
                .HasComment("منطقه")
                .IsRequired()
                .HasColumnName("AddressState");
            address.Property(a => a.Postcode)
                .HasComment("کد پستی")
                .IsRequired()
                .HasColumnName("AddressPostcode");
        });
        builder.HasMany(b => b.Items)
            .WithOne(f => f.Order);
        builder.HasMany(b => b.Totals)
            .WithOne(f => f.Order);
        builder.HasMany(b => b.OrderStateHistory)
            .WithOne(f => f.Order);
        builder.HasMany(b => b.ReturnOrders)
            .WithOne(f => f.Order);
    }
}