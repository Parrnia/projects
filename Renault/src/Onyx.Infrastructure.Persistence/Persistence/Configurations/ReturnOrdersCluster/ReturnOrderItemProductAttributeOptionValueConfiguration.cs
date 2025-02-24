using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ReturnOrdersCluster;
public class ReturnOrderItemProductAttributeOptionValueConfiguration : IEntityTypeConfiguration<ReturnOrderItemGroupProductAttributeOptionValue>
{
    public void Configure(EntityTypeBuilder<ReturnOrderItemGroupProductAttributeOptionValue> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .HasComment("نام گزینه ساختاری")
            .IsRequired();
        builder.Property(t => t.Value)
            .HasComment("مقدار ویژگی محصول")
            .IsRequired();
        builder.Property(t => t.ReturnOrderItemGroupId)
            .IsRequired();
        

        builder.HasOne(b => b.ReturnOrderItemGroup)
            .WithMany(b => b.OptionValues);
    }
}
