namespace Onyx.Domain.Entities.ProductsCluster;
public class Provider : BaseAuditableEntity
{
    /// <summary>
    /// کلید اصلی در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftProviderId { get; set; }
    /// <summary>
    /// فیلد شمارنده
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
    /// شمارنده داخلی
    /// </summary>
    public string? LocalizedCode { get; set; }
    /// <summary>
    /// توضیحات
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// محصولات مرتبط
    /// </summary>
    public List<Product> Products { get; set; } = new List<Product>();
}
