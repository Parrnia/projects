namespace Onyx.Domain.Entities.OrdersCluster.Payments;

public class CashPayment : OrderPayment
{
    /// <inheritdic />
    public override PaymentType PaymentType => PaymentType.Cash;

}