using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.FrontOffice.GetProductAttributeOptionRoles;
public class ProductAttributeOptionRoleByProductAttributeOptionIdDto : IMapFrom<ProductAttributeOptionRole>
{
    public int Id { get; set; }
    public double MinimumStockToDisplayProductForThisType { get; set; }
    public AvailabilityEnum Availability { get; set; }
    public double MainMaxOrderQty { get; set; }
    public double CurrentMaxOrderQty { get; set; }
    public double MainMinOrderQty { get; set; }
    public double CurrentMinOrderQty { get; set; }
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    public double DiscountPercent { get; set; }
}
