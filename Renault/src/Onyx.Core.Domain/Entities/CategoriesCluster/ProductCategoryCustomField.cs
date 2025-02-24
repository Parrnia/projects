namespace Onyx.Domain.Entities.CategoriesCluster;

public class ProductCategoryCustomField : BaseAuditableEntity
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
    /// دسته بندی محصول
    /// </summary>
    public ProductCategory ProductCategory { get; set; } = null!;
    public int ProductCategoryId { get; set; }
}