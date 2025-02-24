namespace Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
public class Badge : BaseAuditableEntity
{
    /// <summary>
    /// مقدار
    /// </summary>
    public string Value { get; set; } = null!;
    /// <summary>
    /// محصولات مرتبط
    /// </summary>
    public List<ProductAttributeOption> ProductAttributeOptions { get; set; } = new List<ProductAttributeOption>();

}
