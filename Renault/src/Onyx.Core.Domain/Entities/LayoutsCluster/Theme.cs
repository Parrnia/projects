namespace Onyx.Domain.Entities.LayoutsCluster;

public class Theme : BaseAuditableEntity
{
    /// <summary>
    ///  عنوان
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    ///  رنگ دکمه اولیه
    /// </summary>
    public string BtnPrimaryColor { get; set; } = null!;
    /// <summary>
    ///  رنگ هاور دکمه اولیه
    /// </summary>
    public string BtnPrimaryHoverColor { get; set; } = null!;
    /// <summary>
    ///  رنگ دکمه ثانویه
    /// </summary>
    public string BtnSecondaryColor { get; set; } = null!;
    /// <summary>
    ///  رنگ هاور دکمه ثانویه
    /// </summary>
    public string BtnSecondaryHoverColor { get; set; } = null!;
    /// <summary>
    ///  رنگ قالب
    /// </summary>
    public string ThemeColor { get; set; } = null!;
    /// <summary>
    /// رنگ هدر و فوتر
    /// </summary>
    public string HeaderAndFooterColor { get; set; } = null!;
    /// <summary>
    /// آیا پیش فرض است؟
    /// </summary>
    public bool IsDefault { get; set; }
}