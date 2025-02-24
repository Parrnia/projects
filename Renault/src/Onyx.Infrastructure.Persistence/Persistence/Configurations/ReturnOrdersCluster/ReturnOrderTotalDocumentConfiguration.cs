using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ReturnOrdersCluster;
public class ReturnOrderTotalDocumentConfiguration : IEntityTypeConfiguration<ReturnOrderTotalDocument>
{
    public void Configure(EntityTypeBuilder<ReturnOrderTotalDocument> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Image)
            .HasComment("تصویر")
            .IsRequired();
        builder.Property(t => t.Description)
            .HasComment("توضیحات")
            .IsRequired();
        builder.Property(t => t.ReturnOrderTotalId)
            .IsRequired();


        builder.HasOne(b => b.ReturnOrderTotal)
            .WithMany(b => b.ReturnOrderTotalDocuments);
    }
}
