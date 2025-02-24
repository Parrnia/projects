using Onyx.Domain.Entities.BlogsCluster;

namespace Onyx.Domain.Entities.CategoriesCluster;

public class BlogCategory : BaseAuditableEntity
{
    public BlogCategory()
    {
        CategoryType = CategoryTypeEnum.BlogCategory;
    }
    /// <summary>
    /// نام فارسی
    /// </summary>
    public string LocalizedName { get; set; } = null!;
    /// <summary>
    /// نام لاتین
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// عنوان کوتاه
    /// </summary>
    public string? Slug { get; set; }
    /// <summary>
    /// تصویر
    /// </summary>
    public Guid Image { get; set; }
    /// <summary>
    /// نوع دسته بندی
    /// </summary>
    public CategoryTypeEnum  CategoryType { get; set; }
    /// <summary>
    /// دسته بندی بلاگ مادر
    /// </summary>
    public BlogCategory? BlogParentCategory { get; set; } = null!;
    public int? BlogParentCategoryId { get; set; }
    /// <summary>
    /// دسته های بلاگ فرزند
    /// </summary>
    public List<BlogCategory>? BlogChildrenCategories { get; set; } = new List<BlogCategory>();
    /// <summary>
    /// فیلدهای اختصاصی
    /// </summary>
    public List<BlogCategoryCustomField> BlogCategoryCustomFields { get; set; } = new List<BlogCategoryCustomField>();
    /// <summary>
    /// پست ها
    /// </summary>
    public List<Post> Posts { get; set; } = new List<Post>();

}