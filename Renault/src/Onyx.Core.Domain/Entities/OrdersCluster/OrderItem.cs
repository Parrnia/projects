using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Domain.Entities.OrdersCluster;

public class OrderItem : BaseAuditableEntity
{
    public OrderItem()
    {
    }
    /// <summary>
    /// قیمت واحد
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// درصد تخفیف محاسبه شده بر روی کالا
    /// </summary>
    public double DiscountPercentForProduct { get; set; } 
    /// <summary>
    /// تعداد
    /// </summary>
    public double Quantity { get; set; }
    /// <summary>
    /// جمع کل قیمت سفارش
    /// </summary>
    public decimal Total { get; set; }
    /// <summary>
    /// سفارش مرتبط
    /// </summary>
    public Order Order { get; set; } = null!;
    /// <summary>
    /// شناسه سفارش مرتبط
    /// </summary>
    public int OrderId { get; set; }
    /// <summary>
    /// نام فارسی قطعه
    /// </summary>
    public string ProductLocalizedName { get; set; } = null!;
    /// <summary>
    /// نام لاتین قطعه
    /// </summary>
    public string ProductName { get; set; } = null!;
    /// <summary>
    /// کد کالا
    /// </summary>
    public string? ProductNo { get; set; }
    /// <summary>
    /// این پراپرتی ها برای آن است که بدانیم مشتری کدام محصول را با چه آپشنی انتخاب کرده است
    /// </summary>
    public ProductAttributeOption ProductAttributeOption { get; set; } = null!;
    /// <summary>
    /// شناسه نوع ویژگی محصول
    /// </summary>
    public int ProductAttributeOptionId { get; set; }
    public List<OrderItemProductAttributeOptionValue> OptionValues { get; set; } = new List<OrderItemProductAttributeOptionValue>();
    /// <summary>
    /// گزینه های سفارش
    /// </summary>
    public List<OrderItemOption> Options { get; set; } = new List<OrderItemOption>();
}