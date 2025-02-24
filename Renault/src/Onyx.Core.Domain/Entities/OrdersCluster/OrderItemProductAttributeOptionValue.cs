namespace Onyx.Domain.Entities.OrdersCluster;
public class OrderItemProductAttributeOptionValue : BaseAuditableEntity
{
    /// <summary>
    /// نام گزینه ساختاری
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// مقدار ویژگی محصول
    /// </summary>
    public string Value { get; set; } = null!;
    public OrderItem OrderItem { get; set; } = null!;
    public int OrderItemId { get; set; }
}
