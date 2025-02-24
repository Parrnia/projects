using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster;
public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.HasIndex(t => t.Name).IsUnique();
        builder.HasIndex(t => t.LocalizedName).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Related7SoftProviderId)
            .HasComment("کلید اصلی در دیتابیس قبلی");
        builder.Property(t => t.Code)
            .HasComment("فیلد شمارنده")
            .IsRequired();
        builder.Property(t => t.LocalizedName).HasComment("نام فارسی")
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(t => t.Name)
            .HasComment("نام لاتین")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(t => t.LocalizedCode)
            .HasComment("شمارنده داخلی")
            .HasMaxLength(20);
        builder.Property(t => t.Description)
            .HasComment("توضیحات")
            .HasMaxLength(500);
        

        builder.HasMany(p => p.Products)
            .WithOne(p => p.Provider);
    }
}
