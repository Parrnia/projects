namespace Onyx.Domain.Entities.ProductsCluster;

public class ProductCustomField : BaseAuditableEntity
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
    /// محصول مرتبط
    /// </summary>
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }
}