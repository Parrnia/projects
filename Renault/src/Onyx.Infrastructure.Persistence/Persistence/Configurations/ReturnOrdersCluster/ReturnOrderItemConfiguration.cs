using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ReturnOrdersCluster;

public class ReturnOrderItemConfiguration : IEntityTypeConfiguration<ReturnOrderItem>
{
    public void Configure(EntityTypeBuilder<ReturnOrderItem> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Quantity)
            .HasComment("تعداد")
            .IsRequired();
        builder.Property(t => t.Total)
            .HasComment("جمع کل قیمت سفارش بازگشتی")
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(t => t.ReturnOrderItemGroupId)
            .HasComment("گروه آیتم سفارش مرتبط")
            .IsRequired();
        builder.Property(t => t.ReturnOrderReasonId)
            .HasComment("دلیل بازگشت کالا")
            .IsRequired();
        builder.Property(t => t.IsAccepted)
            .HasComment("پذیرفته شده")
            .IsRequired();

        builder.HasMany(b => b.ReturnOrderItemDocuments)
            .WithOne(b => b.ReturnOrderItem);

        builder.HasOne(b => b.ReturnOrderItemGroup)
            .WithMany(b => b.ReturnOrderItems);

        builder.OwnsOne(b => b.ReturnOrderReason, returnOrderReason =>
        {
            returnOrderReason.Property(t => t.Id)
                .IsRequired();
            returnOrderReason.Property(t => t.Details)
                .HasComment("توضیحات")
                .IsRequired();
            returnOrderReason.Property(t => t.ReturnOrderReasonType)
                .HasComment("نوع کلی")
                .IsRequired();
            returnOrderReason.Property(t => t.CustomerType)
                .HasComment("نوع دلیل سمت مشتری");
            returnOrderReason.Property(t => t.OrganizationType)
                .HasComment("نوع دلیل سمت سازمان");
        });
    }
}