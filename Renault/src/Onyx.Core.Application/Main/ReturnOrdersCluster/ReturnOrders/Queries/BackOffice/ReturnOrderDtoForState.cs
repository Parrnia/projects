using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Services;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice;
public class ReturnOrderDtoForState : IMapFrom<ReturnOrder>
{
    public int Id { get; set; }
    public ReturnOrderStateBase CurrentReturnOrderStateBase { get; set; } = null!;
    public string CurrentReturnOrderStateName => EnumHelper<ReturnOrderStatus>.GetDisplayValue(CurrentReturnOrderStateBase.ReturnOrderStatus);
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ReturnOrder, ReturnOrderDtoForState>()
            .ForMember(d => d.CurrentReturnOrderStateBase,
                opt =>
                    opt.MapFrom(s => s.GetCurrentReturnOrderState()));
    }
}