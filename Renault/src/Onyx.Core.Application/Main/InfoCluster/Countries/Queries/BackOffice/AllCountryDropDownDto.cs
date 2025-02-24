using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.Countries.Queries.BackOffice;
public class AllCountryDropDownDto : IMapFrom<Country>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
}
