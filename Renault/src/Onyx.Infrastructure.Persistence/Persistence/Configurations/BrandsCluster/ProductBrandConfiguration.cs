using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.BrandsCluster;
public class ProductBrandConfiguration : IEntityTypeConfiguration<ProductBrand>
{
    public void Configure(EntityTypeBuilder<ProductBrand> builder)
    {
        builder.HasIndex(t =>  t.Name).IsUnique();
        builder.HasIndex(t => t.LocalizedName).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Related7SoftBrandId)
            .HasComment("کلید اصلی در دیتابیس قبلی");
        builder.Property(t => t.BrandLogo)
            .HasComment("تصویر لوگو");
        builder.Property(t => t.LocalizedName)
            .HasComment("نام فارسی")
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(t => t.Name)
            .HasComment("نام لاتین")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.Code)
            .HasComment("فیلد شمارنده")
            .IsRequired();
        builder.Property(t => t.Slug)
            .HasComment("عنوان کوتاه")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.CountryId)
            .HasComment("کشور");



        builder.HasMany(b => b.Products)
            .WithOne(f => f.ProductBrand);
        builder.HasOne(b => b.Country)
            .WithMany(f => f.ProductBrands);
    }
}
