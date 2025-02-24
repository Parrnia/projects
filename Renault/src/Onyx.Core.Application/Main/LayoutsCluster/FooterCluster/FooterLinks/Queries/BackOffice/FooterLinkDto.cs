using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice;
public class FooterLinkDto : IMapFrom<FooterLink>
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public bool IsActive { get; set; }
}
