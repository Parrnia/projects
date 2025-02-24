using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.BackOffice;
public class ReturnOrderTotalDto : IMapFrom<ReturnOrderTotal>
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public ReturnOrderTotalType Type { get; set; }
    public string TypeName => EnumHelper<ReturnOrderTotalType>.GetDisplayValue(Type);
    public ReturnOrderTotalApplyType ReturnOrderTotalApplyType { get; set; }
    public string ReturnOrderTotalApplyTypeName => EnumHelper<ReturnOrderTotalApplyType>.GetDisplayValue(ReturnOrderTotalApplyType);
    public int ReturnOrderId { get; set; }
}
