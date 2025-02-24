using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.BrandsCluster;

public class FamilyConfiguration : IEntityTypeConfiguration<Family>
{
    public void Configure(EntityTypeBuilder<Family> builder)
    {
        builder.HasIndex(t => t.Name).IsUnique();
        builder.HasIndex(t => t.LocalizedName).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Related7SoftFamilyId)
            .HasComment("کلید اصلی در دیتابیس قبلی")
            .IsRequired();
        builder.Property(t => t.LocalizedName)
            .HasComment("نام فارسی")
            .IsRequired()
            .HasMaxLength(250);
        builder.Property(t => t.Name)
            .HasComment("نام لاتین")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.VehicleBrandId)
            .HasComment("برند")
            .IsRequired();

        builder.HasOne(f => f.VehicleBrand)
            .WithMany(m => m.Families);
        builder.HasMany(f => f.Models)
            .WithOne(m => m.Family);
    }
}

