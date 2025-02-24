using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrder.GetReturnOrderById;
public class ReturnOrderByIdDto : MainReturnOrderDto, IMapFrom<ReturnOrder>
{
    public Guid CustomerId { get; set; }
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<ReturnOrder, ReturnOrderByIdDto>()
            .ForMember(d => d.CustomerId,
                opt =>
                    opt.MapFrom(s => s.Order.CustomerId));
    }
}