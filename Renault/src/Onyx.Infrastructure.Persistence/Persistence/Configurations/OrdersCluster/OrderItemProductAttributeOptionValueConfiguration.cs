using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.OrdersCluster;
public class OrderItemProductAttributeOptionValueConfiguration : IEntityTypeConfiguration<OrderItemProductAttributeOptionValue>
{
    public void Configure(EntityTypeBuilder<OrderItemProductAttributeOptionValue> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .HasComment("نام گزینه ساختاری")
            .IsRequired();
        builder.Property(t => t.Value)
            .HasComment("مقدار ویژگی محصول")
            .IsRequired();


        builder.HasOne(b => b.OrderItem)
            .WithMany(b => b.OptionValues);
    }
}
