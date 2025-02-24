using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrders.GetReturnOrdersByOrderId;
public class ReturnOrderByOrderIdDto : MainReturnOrderDto, IMapFrom<ReturnOrder>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<ReturnOrder, ReturnOrderByOrderIdDto>();
    }
}