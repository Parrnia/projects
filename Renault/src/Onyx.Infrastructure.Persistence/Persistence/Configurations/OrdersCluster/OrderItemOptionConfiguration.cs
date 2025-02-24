using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.OrdersCluster;
public class OrderItemOptionConfiguration : IEntityTypeConfiguration<OrderItemOption>
{
    public void Configure(EntityTypeBuilder<OrderItemOption> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .HasComment("نام")
            .IsRequired();
        builder.Property(t => t.Value)
            .HasComment("مقدار")
            .IsRequired();
        builder.Property(t => t.OrderItemId)
            .HasComment("آیتم سفارش مرتبط")
            .IsRequired();
        

        builder.HasOne(b => b.OrderItem)
            .WithMany(b => b.Options);
    }
}
