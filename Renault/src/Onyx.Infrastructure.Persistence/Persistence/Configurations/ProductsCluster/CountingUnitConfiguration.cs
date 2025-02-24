using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.ProductsCluster;
public class CountingUnitConfiguration : IEntityTypeConfiguration<CountingUnit>
{
    public void Configure(EntityTypeBuilder<CountingUnit> builder)
    {
        builder.HasIndex(t => t.Name).IsUnique();
        builder.HasIndex(t => t.LocalizedName).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Related7SoftCountingUnitId)
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
        builder.Property(t => t.IsDecimal)
            .HasComment("واحد اعشاری")
            .IsRequired();
        builder.Property(t => t.CountingUnitTypeId)
            .HasComment("نوع واحد شمارنده");

        builder.HasOne(c => c.CountingUnitType)
            .WithMany(c => c.CountingUnits);
        builder.HasMany(c => c.ProductsForMainCountingUnit)
            .WithOne(p => p.MainCountingUnit);
        builder.HasMany(c => c.ProductsForCommonCountingUnit)
            .WithOne(p => p.CommonCountingUnit);
    }
}
