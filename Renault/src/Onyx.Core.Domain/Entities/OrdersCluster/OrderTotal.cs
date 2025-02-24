namespace Onyx.Domain.Entities.OrdersCluster;

public class OrderTotal : BaseAuditableEntity
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
    public OrderTotalType Type { get; set; }
    /// <summary>
    /// سفارش مرتبط
    /// </summary>
    public Order Order { get; set; } = null!;
    /// <summary>
    /// شناسه سفارش مرتبط
    /// </summary>
    public int OrderId { get; set; }
}