using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.BackOffice;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.FrontOffice.GetCountingUnitsWithPagination;
public class CountingUnitWithPaginationDto : IMapFrom<CountingUnit>
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsDecimal { get; set; }
    public CountingUnitTypeDto? CountingUnitType { get; set; }
}
