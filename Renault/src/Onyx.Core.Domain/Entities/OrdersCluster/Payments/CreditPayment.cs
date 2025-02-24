namespace Onyx.Domain.Entities.OrdersCluster.Payments;

public class CreditPayment : OrderPayment
{
    /// <inheritdic />
    public override PaymentType PaymentType => PaymentType.Credit;

}