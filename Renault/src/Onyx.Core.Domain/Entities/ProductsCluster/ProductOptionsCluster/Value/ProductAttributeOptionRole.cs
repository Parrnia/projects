namespace Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
public class ProductAttributeOptionRole : BaseAuditableEntity
{
    /// <summary>
    /// حداقل موجودی کالا برای نمایش کالا به کاربر
    /// </summary>
    public double MinimumStockToDisplayProductForThisCustomerTypeEnum { get; private set; }
    /// <summary>
    /// قابلیت دسترسی
    /// </summary>
    public AvailabilityEnum Availability { get; private set; }
    /// <summary>
    /// حداکثر مقدار سفارش گذاری اصلی
    /// </summary>
    public double MainMaxOrderQty { get; private set; }
    /// <summary>
    /// حداکثر مقدار سفارش گذاری کنونی
    /// </summary>
    public double CurrentMaxOrderQty { get; private set; }
    /// <summary>
    ///  حداقل مقدار سفارش گذاری اصلی
    /// </summary>
    public double MainMinOrderQty { get; private set; }
    /// <summary>
    ///  حداقل مقدار سفارش گذاری کنونی
    /// </summary>
    public double CurrentMinOrderQty { get; private set; }
    /// <summary>
    /// نوع مشتری مربوطه
    /// </summary>
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    /// <summary>
    /// درصد تخفیف روی کالا بر اساس نقش
    /// </summary>
    public double DiscountPercent { get; set; }
    public ProductAttributeOption ProductAttributeOption { get; set; } = null!;
    /// <summary>
    /// شناسه آپشن مربوطه
    /// </summary>
    public int ProductAttributeOptionId { get; set; }
    public void SetMinimumStockToDisplayProductForThisCustomerTypeEnum(double minimumStockToDisplayProductForThisCustomerTypeEnum)
    {
        MinimumStockToDisplayProductForThisCustomerTypeEnum = minimumStockToDisplayProductForThisCustomerTypeEnum;
        SetCurrentMaxOrderQty();
        SetCurrentMinOrderQty();
    }
    public void SetMainMaxOrderQty(double mainMaxOrderQty)
    {
        MainMaxOrderQty = mainMaxOrderQty;
        SetCurrentMaxOrderQty();
    }
    public void SetCurrentMaxOrderQty()
    {
        var countForRole = ProductAttributeOption.TotalCount - MinimumStockToDisplayProductForThisCustomerTypeEnum;
        CurrentMaxOrderQty = Math.Max(Math.Min(MainMaxOrderQty, countForRole), 0);
        SetAvailability();
    }
    public void SetMainMinOrderQty(double mainMinOrderQty)
    {
        MainMinOrderQty = mainMinOrderQty;
        SetCurrentMinOrderQty();
    }
    public void SetCurrentMinOrderQty()
    {
        var countForRole = ProductAttributeOption.TotalCount - MinimumStockToDisplayProductForThisCustomerTypeEnum;
        CurrentMinOrderQty = Math.Max(Math.Min(MainMinOrderQty, countForRole), 0);
        SetAvailability();
    }
    private void SetAvailability()
    {
        var countForRole = ProductAttributeOption.TotalCount - MinimumStockToDisplayProductForThisCustomerTypeEnum;
        var minCurrentOrderQty = Math.Min(CurrentMinOrderQty, CurrentMaxOrderQty);
        Availability = Math.Min(minCurrentOrderQty, countForRole) > 0 ? AvailabilityEnum.InStock : AvailabilityEnum.OutOfStock;
    }
    public bool CheckAvailabilityForRole(double quantity)
    {
        if (quantity > CurrentMaxOrderQty || quantity < CurrentMinOrderQty)
        {
            return false;
        }

        var currentCountForRole = ProductAttributeOption.TotalCount - MinimumStockToDisplayProductForThisCustomerTypeEnum - quantity;
        if (currentCountForRole < 0)
        {
            return false;
        }

        return true;
    }
}
