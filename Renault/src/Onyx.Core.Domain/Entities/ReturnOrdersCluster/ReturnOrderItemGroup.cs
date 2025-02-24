using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Domain.Entities.ReturnOrdersCluster;

public class ReturnOrderItemGroup : BaseAuditableEntity
{
    /// <summary>
    /// قیمت واحد
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// درصد تخفیف محاسبه شده کل
    /// </summary>
    public double TotalDiscountPercent { get; set; }
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
    /// تعداد کل در گروه
    /// </summary>
    public double TotalQuantity { get; private set; }
    /// <summary>
    /// این پراپرتی ها برای آن است که بدانیم مشتری کدام محصول را با چه آپشنی انتخاب کرده است
    /// </summary>
    public ProductAttributeOption ProductAttributeOption { get; set; } = null!;
    public int ProductAttributeOptionId { get; set; }
    public List<ReturnOrderItemGroupProductAttributeOptionValue> OptionValues { get; set; } = new List<ReturnOrderItemGroupProductAttributeOptionValue>();
    public List<ReturnOrderItem> ReturnOrderItems { get; set; } = new List<ReturnOrderItem>();
    public ReturnOrder ReturnOrder { get; set; } = null!;
    public int ReturnOrderId { get; set; }
    //تنها در زمان به روزرسانی کل سفارش بازگشت فراخوانی میشود
    public void SetTotalQuantity()
    {
        TotalQuantity = ReturnOrderItems.Sum(e => e.Quantity);
    }
}