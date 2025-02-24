using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ReturnOrdersCluster;
public class ReturnOrderTotalConfiguration : IEntityTypeConfiguration<ReturnOrderTotal>
{
    public void Configure(EntityTypeBuilder<ReturnOrderTotal> builder)
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
        builder.Property(t => t.Type)
            .HasComment("نوع")
            .IsRequired();
        builder.Property(t => t.ReturnOrderTotalApplyType)
            .HasComment("الگوی اعمال هزینه")
            .IsRequired();
        builder.Property(t => t.ReturnOrderId)
            .HasComment("سفارش بازشگت مرتبط")
            .IsRequired();
        

        builder.HasOne(b => b.ReturnOrder)
            .WithMany(b => b.Totals);
        builder.HasMany(c => c.ReturnOrderTotalDocuments)
            .WithOne(c => c.ReturnOrderTotal);
    }
}
