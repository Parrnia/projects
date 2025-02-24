using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice;
public class PriceDto : IMapFrom<Price>
{
    public PriceDto()
    {
    }
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal MainPrice { get; set; }
    public int ProductAttributeOptionId { get; set; }
}
