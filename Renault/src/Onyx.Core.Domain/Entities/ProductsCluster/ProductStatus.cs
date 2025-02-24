namespace Onyx.Domain.Entities.ProductsCluster;
public class ProductStatus : BaseAuditableEntity
{
    /// <summary>
    /// کلید اصلی در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftProductStatusId { get; set; }
    /// <summary>
    /// فیلد شمارنده
    /// </summary>
    public int Code { get; set; }
    /// <summary>
    /// نام لاتین
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// نام فارسی
    /// </summary>
    public string LocalizedName { get; set; } = null!;
    /// <summary>
    /// محصولات مرتبط
    /// </summary>
    public List<Product>  Products { get; set; } = new List<Product>();
}
