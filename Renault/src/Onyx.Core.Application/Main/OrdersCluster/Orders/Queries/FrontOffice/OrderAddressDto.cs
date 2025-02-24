using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice;

public class OrderAddressDto : IMapFrom<OrderAddress>
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Company { get; set; }
    public string Country { get; set; } = null!;
    public string AddressDetails1 { get; set; } = null!;
    public string? AddressDetails2 { get; set; }
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Postcode { get; set; } = null!;
}