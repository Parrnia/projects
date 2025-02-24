using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Providers.Queries.FrontOffice.GetProvider;
public class ProviderByIdDto : IMapFrom<Provider>
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? LocalizedCode { get; set; }
    public string? Description { get; set; }
}
