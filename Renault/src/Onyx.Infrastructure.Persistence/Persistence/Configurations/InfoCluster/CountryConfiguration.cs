using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.InfoCluster;
public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasIndex(t => t.Name).IsUnique();
        builder.HasIndex(t => t.LocalizedName).IsUnique();


        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Related7SoftCountryId)
            .HasComment("کلید اصلی در دیتابیس قبلی");
        builder.Property(t => t.Code)
            .HasComment("فیلد شمارنده")
            .IsRequired();
        builder.Property(t => t.LocalizedName)
            .HasComment("نام فارسی")
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(t => t.Name)
            .HasComment("نام لاتین")
            .HasMaxLength(50)
            .IsRequired();
        

        builder.HasMany(p => p.Addresses)
            .WithOne(p => p.Country);
        builder.HasMany(p => p.ProductBrands)
            .WithOne(p => p.Country);
        builder.HasMany(p => p.VehicleBrands)
            .WithOne(p => p.Country);
        builder.HasMany(p => p.Products)
            .WithOne(p => p.Country);
    }
}
