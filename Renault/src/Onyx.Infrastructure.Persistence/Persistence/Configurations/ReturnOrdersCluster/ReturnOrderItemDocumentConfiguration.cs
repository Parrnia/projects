using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ReturnOrdersCluster;
public class ReturnOrderItemDocumentConfiguration : IEntityTypeConfiguration<ReturnOrderItemDocument>
{
    public void Configure(EntityTypeBuilder<ReturnOrderItemDocument> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Image)
            .HasComment("تصویر")
            .IsRequired();
        builder.Property(t => t.Description)
            .HasComment("توضیحات")
            .IsRequired();
        builder.Property(t => t.ReturnOrderItemId)
            .IsRequired();
        

        builder.HasOne(b => b.ReturnOrderItem)
            .WithMany(b => b.ReturnOrderItemDocuments);
    }
}
