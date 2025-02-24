using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.LayoutsCluster;

public class ThemeConfiguration : IEntityTypeConfiguration<Theme>
{
    public void Configure(EntityTypeBuilder<Theme> builder)
    {
        builder.HasIndex(t => t.Title).IsUnique();

        builder.Property(t => t.Title)
            .HasComment("عنوان")
            .IsRequired();
        builder.Property(t => t.BtnPrimaryColor)
            .HasComment("رنگ دکمه اولیه")
            .IsRequired();
        builder.Property(t => t.BtnPrimaryHoverColor)
            .HasComment("رنگ هاور دکمه اولیه")
            .IsRequired();
        builder.Property(t => t.BtnSecondaryColor)
            .HasComment("رنگ دکمه ثانویه")
            .IsRequired();
        builder.Property(t => t.BtnSecondaryHoverColor)
            .HasComment("رنگ هاور دکمه ثانویه")
            .IsRequired();
        builder.Property(t => t.ThemeColor)
            .HasComment("رنگ قالب")
            .IsRequired();
        builder.Property(t => t.HeaderAndFooterColor)
            .HasComment("رنگ هدر و فوتر")
            .IsRequired();
    }
}
