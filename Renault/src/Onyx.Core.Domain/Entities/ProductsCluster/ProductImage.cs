namespace Onyx.Domain.Entities.ProductsCluster;
public class ProductImage : BaseAuditableEntity
{
    /// <summary>
    /// آدرس فایل
    /// </summary>
    public Guid Image { get; set; }
    /// <summary>
    /// ترتیب
    /// </summary>
    public int Order { get; set; }
    /// <summary>
    /// محصول مرتبط
    /// </summary>
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }
}
