using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onyx.Domain.Entities.OrdersCluster.Payments;

namespace Onyx.Infrastructure.Persistence.Configurations.OrdersCluster;

public class OrderPaymentConfiguration : IEntityTypeConfiguration<OrderPayment>
{
    public void Configure(EntityTypeBuilder<OrderPayment> builder)
    {
        builder.HasDiscriminator()
            .HasValue<CashPayment>(nameof(CashPayment))
            .HasValue<CreditPayment>(nameof(CreditPayment))
            .HasValue<OnlinePayment>(nameof(OnlinePayment));
        
        builder.Ignore(i => i.PaymentType);

        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Amount)
            .HasComment("مبلغ پراختی");
        builder.HasOne(i => i.Order)
            .WithMany(c => c.PaymentMethods);
    }
}

public class CreditPaymentConfiguration : IEntityTypeConfiguration<CreditPayment>
{
    public void Configure(EntityTypeBuilder<CreditPayment> builder)
    {
    }
}

public class CashPaymentConfiguration : IEntityTypeConfiguration<CashPayment>
{
    public void Configure(EntityTypeBuilder<CashPayment> builder)
    {
    }
}

public class OnlinePaymentConfiguration : IEntityTypeConfiguration<OnlinePayment>
{
    public void Configure(EntityTypeBuilder<OnlinePayment> builder)
    {
    }
}



