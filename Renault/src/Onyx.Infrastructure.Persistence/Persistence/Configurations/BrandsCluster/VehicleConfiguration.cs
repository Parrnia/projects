using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.BrandsCluster;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.VinNumber)
            .HasComment("شماره vin")
            .HasMaxLength(17);
        builder.Property(t => t.CustomerId)
            .IsRequired();
        builder.Property(t => t.KindId)
            .IsRequired();

        builder.HasOne(f => f.Kind)
            .WithMany(m => m.Vehicles);
        builder.HasOne(f => f.Customer)
            .WithMany(m => m.Vehicles);
    }
}