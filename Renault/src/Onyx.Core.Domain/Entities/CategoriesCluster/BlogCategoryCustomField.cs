namespace Onyx.Domain.Entities.CategoriesCluster;

public class BlogCategoryCustomField : BaseAuditableEntity
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
    /// دسته بندی بلاگ
    /// </summary>
    public BlogCategory BlogCategory { get; set; } = null!;
    public int BlogCategoryId { get; set; }
}