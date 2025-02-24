namespace Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;

public class ProductOptionColorCustomField : BaseAuditableEntity
{
    /// <summary>
    /// نام
    /// </summary>
    public string FieldName { get; set; } = null!;
    /// <summary>
    /// مقدار فیلد
    /// </summary>
    public string Value { get; set; } = null!;
    /// <summary>
    /// ویژگی رنگ محصول
    /// </summary>
    public ProductOptionColor ProductOptionColor { get; set; } = null!;
    public int ProductOptionColorId { get; set; }
}