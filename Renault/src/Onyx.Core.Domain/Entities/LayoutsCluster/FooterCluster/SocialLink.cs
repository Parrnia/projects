namespace Onyx.Domain.Entities.LayoutsCluster.FooterCluster;
public class SocialLink : BaseAuditableEntity
{
    /// <summary>
    /// آدرس url
    /// </summary>
    public string Url { get; set; } = null!;
    /// <summary>
    /// آیکون
    /// </summary>
    public Guid Icon { get; set; }
}
