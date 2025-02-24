namespace Onyx.Domain.Entities.OrdersCluster;

public class OrderItemOption : BaseAuditableEntity
{
    /// <summary>
    /// نام
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// مقدار
    /// </summary>
    public string Value { get; set; } = null!;
    /// <summary>
    /// آیتم سفارش مرتبط
    /// </summary>
    public OrderItem OrderItem { get; set; } = null!;
    public int OrderItemId { get; set; }
}