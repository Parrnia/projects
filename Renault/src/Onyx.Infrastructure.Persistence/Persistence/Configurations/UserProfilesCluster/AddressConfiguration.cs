using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.UserProfilesCluster;
public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasIndex(t => new { t.Title, t.CustomerId }).IsUnique().HasFilter("CustomerId IS NOT NULL"); ;
        builder.HasIndex(t => new { t.Postcode, t.CustomerId }).IsUnique().HasFilter("CustomerId IS NOT NULL"); ;


        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Title)
            .HasComment("عنوان")
            .IsRequired();
        builder.Property(t => t.Company)
            .HasComment("شرکت");
        builder.Property(t => t.CountryId)
            .HasComment("کشور")
            .IsRequired();
        builder.Property(t => t.AddressDetails1)
            .HasComment("جزئیات آدرس")
            .IsRequired();
        builder.Property(t => t.City)
            .HasComment("شهر")
            .IsRequired();
        builder.Property(t => t.State)
            .HasComment("منطقه")
            .IsRequired();
        builder.Property(t => t.Postcode)
            .HasComment("کد پستی")
            .IsRequired();
        builder.Property(t => t.Default)
            .HasComment("پیش فرض است؟")
            .IsRequired();
        builder.Property(t => t.CustomerId)
            .HasComment("مشتری مرتبط")
            .IsRequired();

        builder.HasOne(b => b.Customer)
            .WithMany(b => b.Addresses);
        builder.HasOne(b => b.Country)
            .WithMany(f => f.Addresses);
    }
}
