namespace Onyx.Domain.Entities.InfoCluster;
public class TeamMember : BaseAuditableEntity
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
    /// درباره ما مربوطه
    /// </summary>
    public AboutUs AboutUs { get; set; } = null!;
    public int AboutUsId { get; set; }
}
