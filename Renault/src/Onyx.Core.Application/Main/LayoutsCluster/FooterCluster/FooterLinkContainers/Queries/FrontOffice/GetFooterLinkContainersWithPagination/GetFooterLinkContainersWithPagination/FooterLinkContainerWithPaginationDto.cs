using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Queries.FrontOffice.GetFooterLinkContainersWithPagination.GetFooterLinkContainersWithPagination;
public class FooterLinkContainerWithPaginationDto : IMapFrom<FooterLinkContainer>
{
    public int Id { get; set; }
    public string Header { get; set; } = null!;
    public IList<FooterLinkDto> Links { get; set; } = new List<FooterLinkDto>();
}