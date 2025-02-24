namespace Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

public class ProductOptionValueMaterial : BaseAuditableEntity
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
    /// ویژگی جنس محصول
    /// </summary>
    public ProductOptionMaterial ProductOptionMaterial { get; set; } = null!;
    public int ProductOptionMaterialId { get; set; }
}