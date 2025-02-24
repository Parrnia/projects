namespace Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
public class ProductAttributeOptionValue : BaseAuditableEntity
{
    /// <summary>
    /// نام گزینه ساختاری
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// مقدار ویژگی محصول
    /// </summary>
    public string Value { get; set; } = null!;
    public ProductAttributeOption ProductAttributeOption { get; set; } = null!;
    public int ProductAttributeOptionId { get; set; }
}
