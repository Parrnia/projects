namespace Onyx.Domain.Entities.LayoutsCluster;

public class Carousel : BaseAuditableEntity
{
    /// <summary>
    /// آدرس url
    /// </summary>
    public string Url { get; set; } = null!;
    /// <summary>
    /// تصویر دسکتاپ
    /// </summary>
    public Guid DesktopImage { get; set; }
    /// <summary>
    /// تصویر موبایل
    /// </summary>
    public Guid MobileImage { get; set; }
    /// <summary>
    /// تخفیف
    /// </summary>
    public string? Offer { get; set; }
    /// <summary>
    /// عنوان
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// جزئیات
    /// </summary>
    public string Details { get; set; } = null!;
    /// <summary>
    /// برچسب دکمه
    /// </summary>
    public string ButtonLabel { get; set; } = null!;
    /// <summary>
    /// ترتیب
    /// </summary>
    public int Order { get; set; }
}