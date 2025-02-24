namespace Onyx.Domain.Entities.ProductsCluster;
public class CountingUnit : BaseAuditableEntity
{
    /// <summary>
    /// کلید اصلی در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftCountingUnitId { get; set; }
    /// <summary>
    /// کد شمارنده
    /// </summary>
    public int Code { get; set; }
    /// <summary>
    /// نام فارسی 
    /// </summary>
    public string LocalizedName { get; set; } = null!;
    /// <summary>
    /// نام لاتین 
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// واحد اعشاری
    /// </summary>
    public bool IsDecimal { get; set; }
    /// <summary>
    /// محصولات مرتبط
    /// </summary>
    public List<Product> ProductsForMainCountingUnit { get; set; } = new List<Product>();
    public List<Product> ProductsForCommonCountingUnit { get; set; } = new List<Product>();
    /// <summary>
    /// نوع واحد شمارنده
    /// </summary>
    public CountingUnitType? CountingUnitType { get; set; }
    public int? CountingUnitTypeId { get; set; }

}
