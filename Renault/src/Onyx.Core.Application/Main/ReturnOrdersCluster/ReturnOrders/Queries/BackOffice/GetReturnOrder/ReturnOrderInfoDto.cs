using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrder;
public class ReturnOrderInfoDto : IMapFrom<ReturnOrder>
{
    public int Id { get; set; }
    public string PhoneNumber { get; init; } = null!;
    public string CustomerFirstName { get; set; } = null!;
    public string CustomerLastName { get; set; } = null!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ReturnOrder, ReturnOrderInfoDto>()
            .ForMember(d => d.PhoneNumber,
                opt =>
                    opt.MapFrom(s => s.Order.PhoneNumber))
            .ForMember(d => d.CustomerFirstName,
                opt =>
                    opt.MapFrom(s => s.Order.CustomerFirstName))
            .ForMember(d => d.CustomerLastName,
                opt =>
                    opt.MapFrom(s => s.Order.CustomerLastName));
    }
}