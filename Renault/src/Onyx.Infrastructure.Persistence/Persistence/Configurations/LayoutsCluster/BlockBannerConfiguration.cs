using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.LayoutsCluster;

public class BlockBannerConfiguration : IEntityTypeConfiguration<BlockBanner>
{
    public void Configure(EntityTypeBuilder<BlockBanner> builder)
    {
        builder.HasIndex(t => t.BlockBannerPosition).IsUnique();

        builder.Property(t => t.Title)
            .HasComment("عنوان")
            .IsRequired();
        builder.Property(t => t.Subtitle)
            .HasComment("رنگ دکمه اولیه")
            .IsRequired();
        builder.Property(t => t.ButtonText)
            .HasComment("متن کلید")
            .IsRequired();
        builder.Property(t => t.Image)
            .HasComment("تصویر")
            .IsRequired();
        builder.Property(t => t.BlockBannerPosition)
            .HasComment("موقعیت روی صفحه");
    }
}
