using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
public class ProductAttributeOption : BaseAuditableEntity
{
    /// <summary>
    /// تعداد
    /// </summary>
    public double TotalCount { get; private set; }
    /// <summary>
    /// مقدار ذخیره احتیاطی
    /// </summary>
    public double SafetyStockQty { get; set; }
    /// <summary>
    /// مقدار حداقل موجودی
    /// </summary>
    public double MinStockQty { get; set; }
    /// <summary>
    /// مقدار حداکثر موجودی
    /// </summary>
    public double MaxStockQty { get; set; }
    /// <summary>
    /// قیمت های محصول
    /// </summary>
    public List<Price> Prices { get; set; } = new List<Price>();
    /// <summary>
    /// سقف قیمت فروش کالای غیر شرکتی-درصد
    /// </summary>
    public double? MaxSalePriceNonCompanyProductPercent { get; set; }
    /// <summary>
    /// نشان ها
    /// </summary>
    public List<Badge> Badges { get; set; } = new List<Badge>();
    public List<ProductAttributeOptionValue> OptionValues { get; set; } = new List<ProductAttributeOptionValue>();
    /// <summary>
    /// آیا این ویژگی به عنوان پیش فرض نمایش داده خواهد شد؟
    /// </summary>
    public bool IsDefault { get; set; }
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }
    public List<ProductAttributeOptionRole> ProductAttributeOptionRoles { get; set; } = new List<ProductAttributeOptionRole>();
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public List<ReturnOrderItemGroup> ReturnOrderItemGroups { get; set; } = new List<ReturnOrderItemGroup>();

    public void SetTotalCount(double totalCount)
    {
        TotalCount = totalCount;
        foreach (var productAttributeOptionRole in ProductAttributeOptionRoles)
        {
            productAttributeOptionRole.SetCurrentMaxOrderQty();
            productAttributeOptionRole.SetCurrentMinOrderQty();
        }
    }

    public void SetPrice(decimal price)
    {
        var currentPrice = Prices.OrderByDescending(c => c.Date).FirstOrDefault();
        if (currentPrice == null || currentPrice.MainPrice != price)
        {
            Prices.Add(new Price()
            {
                MainPrice = price,
                Date = DateTime.Now
            });
        }
    }

    public decimal GetPrice()
    {
        if (Prices.Count > 0)
        {
            return Prices.OrderBy(c => c.Date).Last().MainPrice;
        }
        return 0;
    }
    
    public bool Sell(double quantity, CustomerTypeEnum customerTypeEnum)
    {
        var isAvailable = CheckAvailability(quantity, customerTypeEnum);
        if (!isAvailable)
        {
            return false;
        }

        var currentTotalCount = TotalCount - quantity;
        if (currentTotalCount < 0)
        {
            return false;
        }
        //تعداد قطعات در این آپشن و همه نقش ها به روز می شود
        SetTotalCount(currentTotalCount);


        return true;
    }

    public void Return(double quantity)
    {
        var currentTotalCount = TotalCount + quantity;
        SetTotalCount(currentTotalCount);
    }

    public bool CheckAvailability(double quantity, CustomerTypeEnum customerTypeEnum)
    {
        //چک می شود که این کالا برای این نوع مشتری وجود داشته باشد
        var productAttributeOptionRole = ProductAttributeOptionRoles.SingleOrDefault(c => c.CustomerTypeEnum == customerTypeEnum);
        if (productAttributeOptionRole == null)
        {
            return false;
        }


        //چک می شود که برای همه نوع های مشتری این کالا موجود باشد
        var currentTotalCount = TotalCount - quantity;
        if (currentTotalCount < 0)
        {
            return false;
        }

        //چک می شود که برای این نوع مشتری موجود باشد
        var isAvailableForThisRole = productAttributeOptionRole.CheckAvailabilityForRole(quantity);
        if (!isAvailableForThisRole)
        {
            return false;
        }

        return true;
    }

}
