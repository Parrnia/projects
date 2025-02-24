namespace Onyx.Domain.Entities.OrdersCluster.Payments;
public abstract class OrderPayment : BaseAuditableEntity
{
    /// <summary>
    /// نوع پرداخت
    /// </summary>
    public abstract PaymentType PaymentType { get; }

    /// <summary>
    /// مبلغ پرداخت شده توسط مشتری.
    /// مجموع این مقدار در پرداخت های سفارش باید با مبلغ سفارش برابر باشد.
    /// </summary>
    public long? Amount { get; set; }

    public Order Order { get; set; } = null!;
    public int OrderId { get; set; }

    public static OrderPayment Create(PaymentType type, long? amount, PaymentServiceType? paymentServiceType)
    {
        switch (type)
        {
            case PaymentType.Cash:
                return new CashPayment()
                {
                    Amount = amount
                };
            case PaymentType.Credit:
                return new CreditPayment()
                {
                    Amount = amount
                };
            case PaymentType.Online:
                return new OnlinePayment()
                {
                    Amount = amount,
                    Status = OnlinePaymentStatus.Waiting,
                    PaymentServiceType = paymentServiceType!.Value
                };
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "نوع پرداخت نامعتبر است");
        }
    }
}

