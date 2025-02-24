namespace Onyx.Domain.Entities.LayoutsCluster.HeaderCluster;

public class Link : BaseAuditableEntity
{
    /// <summary>
    /// عنوان
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// آدرس url
    /// </summary>
    public string Url { get; set; } = null!;
    /// <summary>
    /// تصویر
    /// </summary>
    public Guid? Image { get; set; } = null!;
    /// <summary>
    /// شناسه دسته بندی محصول مرتبط
    /// </summary>
    public int RelatedProductCategoryId { get; set; }
    /// <summary>
    /// لینک مادر
    /// </summary>
    public Link? ParentLink { get; set; } = null!;
    public int? ParentLinkId { get; set; }
    /// <summary>
    /// لینک های فرزند
    /// </summary>
    public List<Link> ChildLinks { get; set; } = new List<Link>();
}
