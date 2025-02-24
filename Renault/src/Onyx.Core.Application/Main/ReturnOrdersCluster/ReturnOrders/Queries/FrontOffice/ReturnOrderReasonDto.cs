using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice;

public class ReturnOrderReasonDto : IMapFrom<ReturnOrderReason>
{
    public int Id { get; set; }
    public string Details { get; set; } = null!;
    public ReturnOrderReasonType ReturnOrderReasonType { get; set; }
    public string ReturnOrderReasonTypeName => EnumHelper<ReturnOrderReasonType>.GetDisplayValue(ReturnOrderReasonType);
    public ReturnOrderCustomerReasonType? CustomerType { get; set; }
    public string? CustomerTypeName => CustomerType != null ? EnumHelper<ReturnOrderCustomerReasonType>.GetDisplayValue((ReturnOrderCustomerReasonType) CustomerType) : null;
    public ReturnOrderOrganizationReasonType? OrganizationType { get; set; }
    public string? OrganizationTypeName => OrganizationType != null ? EnumHelper<ReturnOrderOrganizationReasonType>.GetDisplayValue((ReturnOrderOrganizationReasonType) OrganizationType) : null;
}