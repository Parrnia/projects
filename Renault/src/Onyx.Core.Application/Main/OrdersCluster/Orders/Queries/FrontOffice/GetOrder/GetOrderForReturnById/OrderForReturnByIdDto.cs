using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrder.GetOrderForReturnById;
public class OrderForReturnByIdDto : MainOrderDto, IMapFrom<Order>
{
    public List<OrderItemForReturnByIdDto> ItemsForReturn { get; set; } = new List<OrderItemForReturnByIdDto>();
    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Order, OrderForReturnByIdDto>()
            //.ForMember(i => i.PaymentType, opt =>
            //    opt.MapFrom(s => s.PaymentMethods != null ? s.PaymentMethods.PaymentType : (PaymentType)(-1)))
            .ForMember(c => c.ItemsForReturn, opt => 
                opt.MapFrom(e => e.Items));
    }
}