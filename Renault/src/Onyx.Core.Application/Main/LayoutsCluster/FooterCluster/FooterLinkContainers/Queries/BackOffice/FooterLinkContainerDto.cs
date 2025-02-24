using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.LayoutsCluster.FooterCluster;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Queries.BackOffice;
public class FooterLinkContainerDto : IMapFrom<FooterLinkContainer>
{
    public int Id { get; set; }
    public string Header { get; set; } = null!;
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
