namespace Onyx.Domain.Entities.InfoCluster;

public class CorporationInfo : BaseAuditableEntity
{
    /// <summary>
    /// پیام ارتباط با ما
    /// </summary>
    public string ContactUsMessage { get; set; } = null!;
    /// <summary>
    /// شماره تماس
    /// </summary>
    public List<string> PhoneNumbers { get; set; } = new List<string>();
    /// <summary>
    /// آدرس ایمیل
    /// </summary>
    public List<string> EmailAddresses { get; set; } = new List<string>();
    /// <summary>
    /// آدرس
    /// </summary>
    public List<string> LocationAddresses { get; set; } = new List<string>();
    /// <summary>
    /// ساعات کاری
    /// </summary>
    public List<string> WorkingHours { get; set; } = new List<string>();
    /// <summary>
    /// حامی
    /// </summary>
    public string PoweredBy { get; set; } = null!;
    /// <summary>
    /// تماس با ما
    /// </summary>
    public string CallUs { get; set; } = null!;
    /// <summary>
    /// لوگوی دسکتاپ
    /// </summary>
    public Guid DesktopLogo { get; set; }
    /// <summary>
    /// لوگوی تلفن همراه
    /// </summary>
    public Guid MobileLogo { get; set; }
    /// <summary>
    /// لوگو فوتر
    /// </summary>
    public Guid FooterLogo { get; set; }
    /// <summary>
    /// تصویر پس زمینه اسلایدر
    /// </summary>
    public Guid SliderBackGroundImage { get; set; }
    /// <summary>
    /// آیا پیش فرض است؟
    /// </summary>
    public bool IsDefault { get; set; }
}