using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Providers.Queries.BackOffice;
public class AllProviderDropDownDto : IMapFrom<Provider>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
}
