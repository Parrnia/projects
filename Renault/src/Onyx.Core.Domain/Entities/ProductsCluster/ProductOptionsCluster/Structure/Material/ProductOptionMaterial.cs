namespace Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;

public class ProductOptionMaterial : BaseAuditableEntity
{
    public ProductOptionMaterial()
    {
        Type = ProductOptionTypeEnum.Material;
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
    /// مقدارهای گزینه جنس محصول
    /// </summary>
    public List<ProductOptionValueMaterial> Values { get; set; } = new List<ProductOptionValueMaterial>();
    /// <summary>
    /// فیلدهای اختصاصی
    /// </summary>
    public List<ProductOptionMaterialCustomField> ProductOptionMaterialCustomFields { get; set; } = new List<ProductOptionMaterialCustomField>();
    /// <summary>
    /// محصولات مرتبط
    /// </summary>
    public List<Product> Products { get; set; } = new List<Product>();
}