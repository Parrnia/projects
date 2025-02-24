namespace Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

public class ProductOptionValueColor : BaseAuditableEntity
{
    /// <summary>
    /// نام
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// عنوان کوتاه
    /// </summary>
    public string Slug { get; set; } = null!;
    /// <summary>
    /// رنگ
    /// </summary>
    public string Color { get; set; } = null!;
    /// <summary>
    /// ویژگی رنگ محصول
    /// </summary>
    public ProductOptionColor ProductOptionColor { get; set; } = null!;
    public int ProductOptionColorId { get; set; }
}