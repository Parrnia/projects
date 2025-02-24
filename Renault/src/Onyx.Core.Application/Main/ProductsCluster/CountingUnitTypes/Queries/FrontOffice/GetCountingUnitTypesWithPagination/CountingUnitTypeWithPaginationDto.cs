using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.FrontOffice.GetCountingUnitTypesWithPagination;
public class CountingUnitTypeWithPaginationDto : IMapFrom<CountingUnitType>
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; } = null!;
    public string LocalizedName { get; set; } = null!;
}
