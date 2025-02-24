using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.CustomerSupportCluster;

namespace Onyx.Infrastructure.Persistence.Configurations.CustomerSupportCluster;
public class CustomerTicketConfiguration : IEntityTypeConfiguration<CustomerTicket>
{
    public void Configure(EntityTypeBuilder<CustomerTicket> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();

        builder.Property(t => t.Subject)
            .HasComment("موضوع")
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(t => t.Message)
            .HasComment("پیام")
            .HasMaxLength(1000)
            .IsRequired();
        builder.Property(t => t.Date)
            .HasComment("تاریخ ثبت")
            .IsRequired();
        builder.Property(t => t.CustomerId)
            .HasComment("شناسه مشتری")
            .IsRequired();
        builder.Property(t => t.CustomerPhoneNumber)
            .HasComment("شماره تماس مشتری")
            .IsRequired();
        builder.Property(t => t.CustomerName)
            .HasComment("نام مشتری")
            .IsRequired();

        builder.HasOne(b => b.Customer)
            .WithMany(f => f.CustomerTickets);
    }
}
