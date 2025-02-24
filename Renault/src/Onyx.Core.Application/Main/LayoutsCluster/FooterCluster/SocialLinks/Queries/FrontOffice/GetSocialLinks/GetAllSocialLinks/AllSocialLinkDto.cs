using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Queries.FrontOffice.GetSocialLinks.GetAllSocialLinks;
public class AllSocialLinkDto : IMapFrom<SocialLink>
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public Guid Icon { get; set; }
}