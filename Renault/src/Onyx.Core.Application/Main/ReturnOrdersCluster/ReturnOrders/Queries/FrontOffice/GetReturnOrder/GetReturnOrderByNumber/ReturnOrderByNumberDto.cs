using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrder.GetReturnOrderByNumber;
public class ReturnOrderByNumberDto : MainReturnOrderDto, IMapFrom<ReturnOrder>
{
    public string PhoneNumber { get; set; } = null!;
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<ReturnOrder, ReturnOrderByNumberDto>()
            .ForMember(d => d.PhoneNumber,
            opt =>
                opt.MapFrom(s => s.Order.PhoneNumber));
    }
}