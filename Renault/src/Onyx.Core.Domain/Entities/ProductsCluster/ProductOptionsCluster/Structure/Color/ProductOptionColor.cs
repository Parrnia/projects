namespace Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

public class ProductOptionColor : BaseAuditableEntity
{
    public ProductOptionColor()
    {
        Type = ProductOptionTypeEnum.Color;
    }
    /// <summary>
    /// نام
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// عنوان کوتاه
    /// </summary>
    public string Slug { get; set; } = null!;
    /// <summary>
    /// نوع گزینه محصول
    /// </summary>
    public ProductOptionTypeEnum Type { get; }
    /// <summary>
    /// مقدارهای گزینه رنگ محصول
    /// </summary>
    public List<ProductOptionValueColor> Values { get; set; } = new List<ProductOptionValueColor>();
    /// <summary>
    /// فیلدهای اختصاصی
    /// </summary>
    public List<ProductOptionColorCustomField> ProductOptionColorCustomFields { get; set; } = new List<ProductOptionColorCustomField>();
    /// <summary>
    /// محصولات مرتبط
    /// </summary>
    public List<Product> Products { get; set; } = new List<Product>();
}