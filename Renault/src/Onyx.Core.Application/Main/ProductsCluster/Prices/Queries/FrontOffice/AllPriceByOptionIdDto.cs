using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Prices.Queries.FrontOffice;
public class AllPriceByOptionIdDto : IMapFrom<Price>
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal MainPrice { get; set; }
}
