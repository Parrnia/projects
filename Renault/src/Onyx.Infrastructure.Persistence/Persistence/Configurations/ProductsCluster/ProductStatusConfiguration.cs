using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster;
public class ProductStatusConfiguration : IEntityTypeConfiguration<ProductStatus>
{
    public void Configure(EntityTypeBuilder<ProductStatus> builder)
    {
        builder.HasIndex(t => t.Name).IsUnique();
        builder.HasIndex(t => t.LocalizedName).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Related7SoftProductStatusId)
            .HasComment("کلید اصلی در دیتابیس قبلی");
        builder.Property(t => t.Code)
            .HasComment("فیلد شمارنده")
            .IsRequired();
        builder.Property(t => t.LocalizedName).HasComment("نام فارسی")
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(t => t.Name)
            .HasComment("نام لاتین")
            .HasMaxLength(50)
            .IsRequired();
        

        builder.HasMany(p => p.Products)
            .WithOne(p => p.ProductStatus);
    }
}
