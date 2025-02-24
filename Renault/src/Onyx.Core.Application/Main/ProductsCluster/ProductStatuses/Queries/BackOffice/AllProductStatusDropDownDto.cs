using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.BackOffice;
public class AllProductStatusDropDownDto : IMapFrom<ProductStatus>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
}
