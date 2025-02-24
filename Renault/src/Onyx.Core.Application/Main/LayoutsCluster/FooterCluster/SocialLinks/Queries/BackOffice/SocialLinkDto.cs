using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Queries.BackOffice;
public class SocialLinkDto : IMapFrom<SocialLink>
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public Guid Icon { get; set; }
    public bool IsActive { get; set; }
}
