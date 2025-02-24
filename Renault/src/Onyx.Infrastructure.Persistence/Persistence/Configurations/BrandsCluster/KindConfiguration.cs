using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.BrandsCluster;
public class KindConfiguration : IEntityTypeConfiguration<Kind>
{
    public void Configure(EntityTypeBuilder<Kind> builder)
    {
        //builder.HasIndex(t => t.Name).IsUnique();
        //builder.HasIndex(t => t.LocalizedName).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Related7SoftKindId)
            .HasComment("کلید اصلی در دیتابیس قبلی");
        builder.Property(t => t.LocalizedName)
            .HasComment("نام فارسی")
            .IsRequired()
            .HasMaxLength(250);
        builder.Property(t => t.Name)
            .HasComment("نام لاتین")
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(t => t.ModelId)
            .HasComment("مدل")
            .IsRequired();

        builder.HasOne(f => f.Model)
            .WithMany(m => m.Kinds);
        builder.HasMany(f => f.Vehicles)
            .WithOne(m => m.Kind);
        builder.HasMany(f => f.Products)
            .WithMany(m => m.Kinds)
            .UsingEntity<ProductKind>();
    }
}
