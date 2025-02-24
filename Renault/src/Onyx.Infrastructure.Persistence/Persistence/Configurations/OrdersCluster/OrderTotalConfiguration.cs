using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.OrdersCluster;
public class OrderTotalConfiguration : IEntityTypeConfiguration<OrderTotal>
{
    public void Configure(EntityTypeBuilder<OrderTotal> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Title)
            .HasComment("عنوان")
            .IsRequired();
        builder.Property(t => t.Price)
            .HasComment("مبلغ")
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(t => t.OrderId)
            .HasComment("سفارش مرتبط")
            .IsRequired();
        

        builder.HasOne(b => b.Order)
            .WithMany(b => b.Totals);
    }
}
