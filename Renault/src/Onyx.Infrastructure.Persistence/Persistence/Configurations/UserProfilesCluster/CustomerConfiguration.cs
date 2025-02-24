using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.UserProfilesCluster;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasMany(b => b.Addresses)
            .WithOne(b => b.Customer);
        builder.HasMany(b => b.Reviews)
            .WithOne(f => f.Customer);
        builder.HasMany(b => b.Orders)
            .WithOne(f => f.Customer);
        builder.HasMany(b => b.Vehicles)
            .WithOne(f => f.Customer);
        builder.HasMany(b => b.Comments)
            .WithOne(f => f.Author);
        builder.HasMany(b => b.WidgetComments)
            .WithOne(f => f.Author);
        builder.HasMany(b => b.Credits)
            .WithOne(f => f.Customer);
        builder.HasMany(b => b.MaxCredits)
            .WithOne(f => f.Customer);

    }
}
