using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Domain.Entities.ProductsCluster;
public class ProductKind : BaseAuditableEntity
{
    /// <summary>
    /// کلید اصلی در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftProductKindId { get; set; }
    /// <summary>
    /// محصول مرتبط جدول ارتباطی
    /// </summary>
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }
    /// <summary>
    /// کلید اصلی محصول در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftProductId { get; set; }
    /// <summary>
    /// نوع مرتبط جدول ارتباطی
    /// </summary>
    public Kind Kind { get; set; } = null!;
    public int KindId { get; set; }
    /// <summary>
    /// کلید اصلی نوع در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftKindId { get; set; }
}
