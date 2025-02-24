namespace Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

public class ProductOptionMaterialCustomField : BaseAuditableEntity
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
    /// ویژگی جنس محصول
    /// </summary>
    public ProductOptionMaterial ProductOptionMaterial { get; set; } = null!;
    public int ProductOptionMaterialId { get; set; }
}