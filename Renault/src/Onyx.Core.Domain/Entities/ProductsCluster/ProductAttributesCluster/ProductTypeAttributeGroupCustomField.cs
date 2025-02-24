namespace Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

public class ProductTypeAttributeGroupCustomField : BaseAuditableEntity
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
    /// گروه ویژگی پایه
    /// </summary>
    public ProductTypeAttributeGroup ProductTypeAttributeGroup { get; set; } = null!;
    public int ProductTypeAttributeGroupId { get; set; }
}