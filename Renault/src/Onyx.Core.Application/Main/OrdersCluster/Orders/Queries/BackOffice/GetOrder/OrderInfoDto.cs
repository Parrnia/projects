using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrder;
public class OrderInfoDto : IMapFrom<Order>
{
    public int Id { get; set; }
    public string AddressDetails1 { get; set; } = null!;
    public string AddressDetails2 { get; set; } = null!;
    public string Postcode { get; set; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public string CustomerFirstName { get; set; } = null!;
    public string CustomerLastName { get; set; } = null!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Order, OrderInfoDto>()
            .ForMember(d => d.AddressDetails1,
            opt =>
                opt.MapFrom(s => s.OrderAddress.AddressDetails1))
            .ForMember(d => d.AddressDetails2,
            opt =>
                opt.MapFrom(s => s.OrderAddress.AddressDetails2))
            .ForMember(d => d.Postcode,
                opt =>
                    opt.MapFrom(s => s.OrderAddress.Postcode));
    }
}