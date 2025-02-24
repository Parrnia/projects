using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.UserProfilesCluster;
public class CreditConfiguration : IEntityTypeConfiguration<Credit>
{
    [Obsolete]
    public void Configure(EntityTypeBuilder<Credit> builder)
    {
        builder.HasIndex(t => t.OrderToken).IsUnique();

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Date)
            .HasComment("تاریخ ثبت")
            .IsRequired();
        builder.Property(t => t.Value)
            .HasComment("مقدار اعتبار")
            .IsRequired()
            .HasPrecision(18, 2);
        builder.HasCheckConstraint("CK_Credit_Value_NonNegative", "Value >= 0");
        builder.Property(t => t.ModifierUserName)
            .HasComment("نام کاربر تغییردهنده")
            .IsRequired();
        builder.Property(t => t.ModifierUserId)
            .HasComment("شناسه کاربر تغییردهنده")
            .IsRequired();
        builder.Property(t => t.OrderToken)
            .HasComment("شماره سفارش مربوطه");

        builder.HasOne(c => c.Customer)
            .WithMany(c => c.Credits);
    }
}