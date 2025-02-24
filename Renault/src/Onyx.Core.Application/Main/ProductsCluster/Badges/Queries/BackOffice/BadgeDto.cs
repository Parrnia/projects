using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Badges.Queries.BackOffice;
public class BadgeDto : IMapFrom<Badge>
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
    public bool IsActive { get; set; }
}
