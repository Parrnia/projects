using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.FrontOffice.GetKinds.GetKindsByModelId;
public class KindByModelIdDto : IMapFrom<Kind>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
}
