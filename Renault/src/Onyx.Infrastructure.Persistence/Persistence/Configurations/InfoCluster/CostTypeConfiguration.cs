using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.InfoCluster;
public class CostTypeConfiguration : IEntityTypeConfiguration<CostType>
{
    public void Configure(EntityTypeBuilder<CostType> builder)
    {
        builder.HasIndex(t => t.Value).IsUnique();
        builder.HasIndex(t => t.Text).IsUnique();
        builder.HasIndex(t => t.CostTypeEnum).IsUnique();


        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Value)
            .HasComment("مقدار");
        builder.Property(t => t.Text)
            .HasComment("متن")
            .IsRequired();
        builder.Property(t => t.CostTypeEnum)
            .HasComment("نوع هزینه");
    }
}
