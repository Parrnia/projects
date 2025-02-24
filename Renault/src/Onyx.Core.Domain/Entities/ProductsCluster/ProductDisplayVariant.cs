namespace Onyx.Domain.Entities.ProductsCluster;
public class ProductDisplayVariant : BaseAuditableEntity
{
    /// <summary>
    /// نام نمایشی محصول
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// محصول مرتبط
    /// </summary>
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }
    /// <summary>
    /// پرفروش
    /// </summary>
    public bool IsBestSeller { get; set; }
    /// <summary>
    /// ویژه
    /// </summary>
    public bool IsFeatured { get; set; }
    /// <summary>
    /// آخرین
    /// </summary>
    public bool IsLatest { get; set; }
    /// <summary>
    /// جدید
    /// </summary>
    public bool IsNew { get; set; }
    /// <summary>
    /// محبوب
    /// </summary>
    public bool IsPopular { get; set; }
    /// <summary>
    /// حراج
    /// </summary>
    public bool IsSale { get; set; }
    /// <summary>
    /// پیشنهاد ویژه
    /// </summary>
    public bool IsSpecialOffer { get; set; }
    /// <summary>
    /// بالاترین امتیاز
    /// </summary>
    public bool IsTopRated { get; set; }
}