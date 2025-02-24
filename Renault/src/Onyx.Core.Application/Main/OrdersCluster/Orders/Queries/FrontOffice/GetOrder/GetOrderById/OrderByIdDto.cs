using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrder.GetOrderById;
public class OrderByIdDto : MainOrderDto, IMapFrom<Order>
{
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Order, OrderByIdDto>()
        //    .ForMember(i => i.PaymentType, opt =>
        //        opt.MapFrom(s => s.PaymentMethods != null ? s.PaymentMethods.PaymentType : (PaymentType)(-1)))
        ;
    }
}