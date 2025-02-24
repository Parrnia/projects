namespace Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
public class Price : BaseAuditableEntity
{
    /// <summary>
    /// تاریخ قیمت
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// قیمت اصلی
    /// </summary>
    public decimal MainPrice { get; set; }
    /// <summary>
    /// محصول مرتبط
    /// </summary>
    public ProductAttributeOption ProductAttributeOption { get; set; } = null!;
    public int ProductAttributeOptionId { get; set; }
}