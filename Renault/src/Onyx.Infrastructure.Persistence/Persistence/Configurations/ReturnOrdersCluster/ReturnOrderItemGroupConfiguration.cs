using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ReturnOrdersCluster;
public class ReturnOrderItemGroupConfiguration : IEntityTypeConfiguration<ReturnOrderItemGroup>
{
    public void Configure(EntityTypeBuilder<ReturnOrderItemGroup> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Price)
            .HasComment("قیمت واحد")
            .HasPrecision(18, 2)
            .IsRequired();
        builder.Property(t => t.TotalDiscountPercent)
            .HasComment("درصد تخفیف محاسبه شده کل")
            .IsRequired();
        builder.Property(t => t.ProductLocalizedName)
            .HasComment("نام فارسی قطعه")
            .IsRequired();
        builder.Property(t => t.ProductName)
            .HasComment("نام لاتین قطعه")
            .IsRequired();
        builder.Property(t => t.ProductNo)
            .HasComment("کد کالا")
            .IsRequired();
        builder.Property(t => t.ProductAttributeOptionId)
            .IsRequired();
        builder.Property(t => t.ReturnOrderId)
            .IsRequired();
        

        builder.HasOne(b => b.ProductAttributeOption)
            .WithMany(b => b.ReturnOrderItemGroups);
        builder.HasOne(b => b.ReturnOrder)
            .WithMany(b => b.ItemGroups);
        builder.HasMany(b => b.OptionValues)
            .WithOne(b => b.ReturnOrderItemGroup);
        builder.HasMany(b => b.ReturnOrderItems)
            .WithOne(b => b.ReturnOrderItemGroup);
    }
}
