using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice;
public class ProductAttributeOptionValueDto : IMapFrom<ProductAttributeOptionValue>
{
    public int Id { get; set; }
    public int ProductAttributeOptionId { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}
