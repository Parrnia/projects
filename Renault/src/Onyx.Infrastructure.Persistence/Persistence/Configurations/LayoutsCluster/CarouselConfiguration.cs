using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.LayoutsCluster;

public class CarouselConfiguration : IEntityTypeConfiguration<Carousel>
{
    public void Configure(EntityTypeBuilder<Carousel> builder)
    {
        builder.HasIndex(t => t.Title).IsUnique();

        builder.Property(t => t.Url)
            .HasComment("آدرس url")
            .IsRequired();
        builder.Property(t => t.DesktopImage)
            .HasComment("تصویر دسکتاپ")
            .IsRequired();
        builder.Property(t => t.MobileImage)
            .HasComment("تصویر موبایل")
            .IsRequired();
        builder.Property(t => t.Offer)
            .HasComment("تخفیف");
        builder.Property(t => t.Title)
            .HasComment("عنوان")
            .IsRequired();
        builder.Property(t => t.Details)
            .HasComment("جزئیات")
            .IsRequired();
        builder.Property(t => t.ButtonLabel)
            .HasComment("برچسب دکمه")
            .IsRequired();
        builder.Property(t => t.Order)
            .HasComment("ترتیب")
            .IsRequired();
    }
}
