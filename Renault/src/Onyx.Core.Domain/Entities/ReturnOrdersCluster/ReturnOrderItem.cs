namespace Onyx.Domain.Entities.ReturnOrdersCluster;

public class ReturnOrderItem : BaseAuditableEntity
{
    /// <summary>
    /// تعداد
    /// </summary>
    public double Quantity { get; set; }
    /// <summary>
    /// جمع کل قیمت سفارش بازگشتی
    /// </summary>
    public decimal Total { get; private set; }
    /// <summary>
    /// دلیل بازگشت کالا
    /// </summary>
    public ReturnOrderReason ReturnOrderReason { get; set; } = null!;
    public int ReturnOrderReasonId { get; set; }
    /// <summary>
    /// مستندات بازگشت کالاها
    /// </summary>
    public List<ReturnOrderItemDocument> ReturnOrderItemDocuments { get; set; } = new List<ReturnOrderItemDocument>();
    /// <summary>
    /// پذیرفته شده
    /// </summary>
    public bool IsAccepted { get; set; }
    /// <summary>
    /// گروه آیتم سفارش مرتبط
    /// </summary>
    public ReturnOrderItemGroup ReturnOrderItemGroup { get; set; } = null!;
    public int ReturnOrderItemGroupId { get; set; }

    public void SetTotal(decimal price)
    {
        Total = price * (decimal) Quantity;
    }
}