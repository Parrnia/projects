using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.FrontOffice.GetFamilies.GetFamiliesByBrandId;
public class FamilyByBrandIdDto : IMapFrom<Family>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
}
