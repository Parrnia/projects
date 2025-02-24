namespace Onyx.Domain.Entities.LayoutsCluster.FooterCluster;
public class FooterLinkContainer : BaseAuditableEntity
{
    /// <summary>
    /// عنوان
    /// </summary>
    public string Header { get; set; } = null!;
    /// <summary>
    /// لینک ها
    /// </summary>
    public List<FooterLink> Links { get; set; } = new List<FooterLink>();
    /// <summary>
    /// ترتیب
    /// </summary>
    public int Order { get; set; }
}
