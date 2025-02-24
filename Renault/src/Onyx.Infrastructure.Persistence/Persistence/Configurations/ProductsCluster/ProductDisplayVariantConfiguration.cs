using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster;
public class ProductDisplayVariantConfiguration : IEntityTypeConfiguration<ProductDisplayVariant>
{
    public void Configure(EntityTypeBuilder<ProductDisplayVariant> builder)
    {
        builder.HasIndex(pd => new { pd.Name, pd.ProductId })
            .IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .HasComment("نام نمایشی محصول")
            .IsRequired();
        builder.Property(t => t.ProductId)
            .HasComment("محصول مرتبط")
            .IsRequired();
        builder.Property(t => t.IsBestSeller)
            .HasComment("پرفروش")
            .IsRequired();
        builder.Property(t => t.IsFeatured)
            .HasComment("ویژه")
            .IsRequired();
        builder.Property(t => t.IsLatest)
            .HasComment("آخرین")
            .IsRequired();
        builder.Property(t => t.IsNew)
            .HasComment("جدید")
            .IsRequired();
        builder.Property(t => t.IsPopular)
            .HasComment("محبوب")
            .IsRequired();
        builder.Property(t => t.IsSale)
            .HasComment("حراج")
            .IsRequired();
        builder.Property(t => t.IsSpecialOffer)
            .HasComment("پیشنهاد ویژه")
            .IsRequired();
        builder.Property(t => t.IsTopRated)
            .HasComment("بالاترین امتیاز")
            .IsRequired();

        builder.HasOne(b => b.Product)
            .WithMany(b => b.ProductDisplayVariants);
    }
}
