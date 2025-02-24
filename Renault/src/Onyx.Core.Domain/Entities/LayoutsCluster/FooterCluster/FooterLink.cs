namespace Onyx.Domain.Entities.LayoutsCluster.FooterCluster;
public class FooterLink : BaseAuditableEntity
{
    /// <summary>
    /// عنوان
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// آدرس url
    /// </summary>
    public string Url { get; set; } = null!;
    public FooterLinkContainer FooterLinkContainer { get; set; } = null!;
    public int FooterLinkContainerId { get; set; }
}
