namespace Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

public class ProductAttribute : BaseAuditableEntity
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
    /// ویژه
    /// </summary>
    public bool Featured { get; set; }
    /// <summary>
    /// فیلدهای اختصاصی
    /// </summary>
    public List<ProductAttributeCustomField> ProductAttributeCustomFields { get; set; } = new List<ProductAttributeCustomField>();
    /// <summary>
    /// مقدار ویژگی محصول
    /// </summary>
    /// <summary>
    /// نام مقدار
    /// </summary>
    public string ValueName { get; set; } = null!;
    /// <summary>
    /// عنوان کوتاه مقدار
    /// </summary>
    public string ValueSlug { get; set; } = null!;
    /// <summary>
    /// فلدهای اختصاصی مقدار
    /// </summary>
    public List<ProductAttributeValueCustomField> ProductAttributeValueCustomFields { get; set; } = new List<ProductAttributeValueCustomField>();
    /// <summary>
    /// محصول مرتبط
    /// </summary>
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }
}