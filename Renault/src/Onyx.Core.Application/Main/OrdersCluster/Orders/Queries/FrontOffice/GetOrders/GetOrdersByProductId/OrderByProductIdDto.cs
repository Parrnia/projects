using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrders.GetOrdersByProductId;
public class OrderByProductIdDto : MainOrderDto, IMapFrom<Order>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Order, OrderByProductIdDto>()
        //.ForMember(i => i.PaymentType, opt =>
        //        opt.MapFrom(s => s.PaymentMethods != null ? s.PaymentMethods.PaymentType : (PaymentType)(-1)))
        ;
    }
}
