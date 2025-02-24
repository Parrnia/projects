using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.Countries.Queries.FrontOffice.GetCountriesWithPagination;
public class CountryWithPaginationDto : IMapFrom<Country>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Code { get; set; }
}
