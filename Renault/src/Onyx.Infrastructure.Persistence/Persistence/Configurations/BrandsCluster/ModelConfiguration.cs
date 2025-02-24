using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.BrandsCluster;
public class ModelConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        //builder.HasIndex(t => t.Name).IsUnique();
        //builder.HasIndex(t => t.LocalizedName).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Related7SoftModelId)
            .HasComment("کلید اصلی در دیتابیس قبلی");
        builder.Property(t => t.LocalizedName)
            .IsRequired()
            .HasMaxLength(250)
            .HasComment("نام فارسی");
        builder.Property(t => t.Name)
            .HasMaxLength(250)
            .HasComment("نام لاتین")
            .IsRequired();
        builder.Property(t => t.FamilyId)
            .HasComment("خانواده")
            .IsRequired();

        builder.HasOne(m => m.Family)
            .WithMany(k => k.Models);
        builder.HasMany(m => m.Kinds)
            .WithOne(k => k.Model);
    }
}
