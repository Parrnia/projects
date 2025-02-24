namespace Onyx.Domain.Entities.ReturnOrdersCluster;

public class ReturnOrderTotal : BaseAuditableEntity
{
    /// <summary>
    /// عنوان
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// مبلغ
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// نوع
    /// </summary>
    public ReturnOrderTotalType Type { get; set; }
    /// <summary>
    /// الگوی اعمال هزینه
    /// </summary>
    public ReturnOrderTotalApplyType ReturnOrderTotalApplyType { get; set; }
    /// <summary>
    /// مستندات هزینه های جانبی
    /// </summary>
    public List<ReturnOrderTotalDocument> ReturnOrderTotalDocuments { get; set; } = new List<ReturnOrderTotalDocument>();
    /// <summary>
    /// سفارش بازشگت مرتبط
    /// </summary>
    public ReturnOrder ReturnOrder { get; set; } = null!;
    public int ReturnOrderId { get; set; }
}