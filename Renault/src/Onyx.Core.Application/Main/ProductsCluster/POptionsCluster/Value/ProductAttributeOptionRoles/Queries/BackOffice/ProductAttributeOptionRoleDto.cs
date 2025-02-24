using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.BackOffice;
public class ProductAttributeOptionRoleDto : IMapFrom<ProductAttributeOptionRole>
{
    public int Id { get; set; }
    public double MinimumStockToDisplayProductForThisCustomerTypeEnum { get; set; }
    public AvailabilityEnum Availability { get; set; }
    public string AvailabilityName => EnumHelper<AvailabilityEnum>.GetDisplayValue(Availability);
    public double MainMaxOrderQty { get; set; }
    public double CurrentMaxOrderQty { get; set; }
    public double MainMinOrderQty { get; set; }
    public double CurrentMinOrderQty { get; set; }
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    public string CustomerTypeEnumName => EnumHelper<CustomerTypeEnum>.GetDisplayValue(CustomerTypeEnum);
    public double DiscountPercent { get; set; }
}
