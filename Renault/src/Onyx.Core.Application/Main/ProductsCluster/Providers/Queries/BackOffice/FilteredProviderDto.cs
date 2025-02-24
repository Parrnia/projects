namespace Onyx.Application.Main.ProductsCluster.Providers.Queries.BackOffice;
public class FilteredProviderDto
{
    public List<ProviderDto> Providers { get; set; } = new List<ProviderDto>();
    public int Count { get; set; }
}
