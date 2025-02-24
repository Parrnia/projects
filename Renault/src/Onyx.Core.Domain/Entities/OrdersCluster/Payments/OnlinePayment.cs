namespace Onyx.Domain.Entities.OrdersCluster.Payments;

public class OnlinePayment : OrderPayment
{
    /// <inheritdic />
    public override PaymentType PaymentType => PaymentType.Online;

    /// <summary>
    /// نوع درگاه پرداخت
    /// </summary>
    public PaymentServiceType PaymentServiceType { get; set; }

    /// <summary>
    /// وضعبت پرداخت
    /// </summary>
    public OnlinePaymentStatus Status { get; set; }

    /// <summary>
    /// توکن شروع تراکنش
    /// </summary>
    public string? Authority { get; set; }

    /// <summary>
    /// شماره کارت ماسک شده
    /// </summary>
    public string? CardNumber { get; set; }
    
    /// <summary>
    /// شماره پیگیری شبکه بانکی
    /// </summary>
    public string? Rrn { get; set; }

    /// <summary>
    /// توکنی که هنگام ارجاع به درگاه پرداخت ارسال شده است.
    /// این مقدار باید با
    /// authority
    /// برابر باشد
    /// </summary>
    public string? RefId { get; set; }

    /// <summary>
    /// شماره تراکنش سوییچ جهت پیگیری وضعیت پرداخت
    /// </summary>
    public string? PayGateTranId { get; set; }
    
    /// <summary>
    /// شماره فاکتور اعلام شده به بانک.
    /// این مقدار باید با شناسه پرداخت سفارش برابر باشد
    /// </summary>
    public long? SalesOrderId { get; set; }
    
    /// <summary>
    /// نوع خدمت، برای فروش محصول باید برابر با 1 باشد
    /// </summary>
    public int? ServiceTypeId { get; set; }
    
    /// <summary>
    /// خطای اعتبار سنجی تراکنش
    /// </summary>
    public string? Error { get; set; }
}