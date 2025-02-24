namespace Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

public class ProductAttributeValueCustomField : BaseAuditableEntity
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
    /// مقدار ویژگی محصول
    /// </summary>
    public ProductAttribute ProductAttribute { get; set; } = null!;
    public int ProductAttributeId { get; set; }
}