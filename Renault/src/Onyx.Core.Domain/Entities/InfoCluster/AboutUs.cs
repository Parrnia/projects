namespace Onyx.Domain.Entities.InfoCluster;

public class AboutUs : BaseAuditableEntity
{
    /// <summary>
    /// عنوان
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// محتوای متنی
    /// </summary>
    public string TextContent { get; set; } = null!;
    /// <summary>
    /// تصویر
    /// </summary>
    public Guid ImageContent { get; set; }
    /// <summary>
    /// آیا پیش فرض است؟
    /// </summary>
    public bool IsDefault { get; set; }
    /// <summary>
    /// اعضای تیم
    /// </summary>
    public List<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    /// <summary>
    /// شواهد
    /// </summary>
    public List<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

}