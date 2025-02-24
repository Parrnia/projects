using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.BackOffice;
public class AllCountingUnitDropDownDto : IMapFrom<CountingUnit>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
}
