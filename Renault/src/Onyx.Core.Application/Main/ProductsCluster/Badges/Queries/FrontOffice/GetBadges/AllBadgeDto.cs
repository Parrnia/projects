using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Badges.Queries.FrontOffice.GetBadges;
public class AllBadgeDto : IMapFrom<Badge>
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
}
