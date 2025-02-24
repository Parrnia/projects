namespace Onyx.Domain.Entities.LayoutsCluster;

public class BlockBanner : BaseAuditableEntity
{
    /// <summary>
    ///  عنوان
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    ///  زیرعنوان
    /// </summary>
    public string Subtitle { get; set; } = null!;
    /// <summary>
    ///  متن دکمه
    /// </summary>
    public string ButtonText { get; set; } = null!;
    /// <summary>
    /// تصویر
    /// </summary>
    public Guid Image { get; set; }
    /// <summary>
    /// موقعیت روی صفحه
    /// </summary>
    public BlockBannerPosition BlockBannerPosition { get; set; }
}