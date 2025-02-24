using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Domain.Entities.CategoriesCluster;
public class ProductCategory : BaseAuditableEntity
{
    public ProductCategory()
    {
        CategoryType = CategoryTypeEnum.ProductCategory;
    }
    /// <summary>
    /// کلید اصلی در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftProductCategoryId { get; set; }
    /// <summary>
    /// فیلد شمارنده
    /// </summary>
    public int Code { get; set; }
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
    /// شماره دسته کالا
    /// </summary>
    public string? ProductCategoryNo { get; set; }
    /// <summary>
    /// تصویر
    /// </summary>
    public Guid? Image { get; set; }
    /// <summary>
    /// تصویر منو
    /// </summary>
    public Guid? MenuImage { get; set; }
    /// <summary>
    /// نوع دسته بندی
    /// </summary>
    public CategoryTypeEnum CategoryType { get; set; }
    /// <summary>
    /// محبوب
    /// </summary>
    public bool IsPopular { get; set; }
    /// <summary>
    /// ویژه
    /// </summary>
    public bool IsFeatured { get; set; }
    /// <summary>
    /// دسته بندی کالای مادر
    /// </summary>
    public ProductCategory? ProductParentCategory { get; set; } = null!;
    public int? ProductParentCategoryId { get; set; }
    /// <summary>
    /// دسته های کالای فرزند
    /// </summary>
    public List<ProductCategory>? ProductChildrenCategories { get; set; } = new List<ProductCategory>();
    /// <summary>
    /// فیلدهای اختصاصی
    /// </summary>
    public List<ProductCategoryCustomField> ProductCategoryCustomFields { get; set; } = new List<ProductCategoryCustomField>();
    /// <summary>
    /// محصولات
    /// </summary>
    public List<Product>? Products { get; set; } = new List<Product>();

}
