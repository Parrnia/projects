using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.FrontOffice.GetProductTypesWithPagination;
public class ProductTypeWithPaginationDto : IMapFrom<ProductType>
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; } = null!;
    public string LocalizedName { get; set; } = null!;
}
