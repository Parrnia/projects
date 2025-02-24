namespace Onyx.Domain.Entities.ProductsCluster;
public class Tag : BaseAuditableEntity
{
    /// <summary>
    /// عنوان انگلیسی
    /// </summary>
    public string EnTitle { get; set; } = null!;
    /// <summary>
    /// عنوان فارسی
    /// </summary>
    public string FaTitle { get; set; } = null!;
    /// <summary>
    /// محصولات مرتبط
    /// </summary>
    public List<Product> Products { get; set; } = new List<Product>();
}