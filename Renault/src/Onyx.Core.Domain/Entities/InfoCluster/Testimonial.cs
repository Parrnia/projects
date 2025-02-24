namespace Onyx.Domain.Entities.InfoCluster;
public class Testimonial : BaseAuditableEntity
{
    /// <summary>
    /// نام
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// سمت
    /// </summary>
    public string Position { get; set; } = null!;
    /// <summary>
    /// تصویر پروفایل
    /// </summary>
    public Guid Avatar { get; set; }
    /// <summary>
    /// امتیاز
    /// </summary>
    public int Rating { get; set; }
    /// <summary>
    /// دیدگاه
    /// </summary>
    public string Review { get; set; } = null!;
    public AboutUs AboutUs { get; set; } = null!;
    public int AboutUsId { get; set; }
}
