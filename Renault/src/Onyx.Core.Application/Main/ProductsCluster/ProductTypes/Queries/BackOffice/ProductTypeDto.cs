using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.BackOffice;
public class ProductTypeDto : IMapFrom<ProductType>
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; } = null!;
    public string LocalizedName { get; set; } = null!;
}
