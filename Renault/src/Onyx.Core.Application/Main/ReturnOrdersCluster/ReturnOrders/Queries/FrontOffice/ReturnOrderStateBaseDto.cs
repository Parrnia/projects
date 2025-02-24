using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Services;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice;

public class ReturnOrderStateBaseDto : IMapFrom<ReturnOrderStateBase>
{
    public int Id { get; set; }
    public ReturnOrderStatus ReturnOrderStatus { get; set; }
    public string ReturnOrderStatusName => EnumHelper<ReturnOrderStatus>.GetDisplayValue(ReturnOrderStatus);
    public string Details { get; set; } = null!;
    public string Created { get; set; } = null!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ReturnOrderStateBase, ReturnOrderStateBaseDto>()
            .ForMember(d => d.Created,
                opt =>
                    opt.MapFrom(s => s.Created.ToPersianDate()));
    }
}