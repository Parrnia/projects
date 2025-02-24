using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Queries.FrontOffice;
public class CustomerDto : IMapFrom<Customer>
{
    public CustomerDto()
    {
        Addresses = new List<AddressForCustomerDto>();
        Vehicles = new List<VehicleForCustomerDto>();
    }
    public Guid Id { get; set; }
    public string? Company { get; set; }
    public Guid? Avatar { get; set; }
    public List<CreditDto> Credits { get; set; } = new List<CreditDto>();
    public List<MaxCreditDto> MaxCredits { get; set; } = new List<MaxCreditDto>();
    public List<AddressForCustomerDto> Addresses { get; set; } 
    public List<VehicleForCustomerDto> Vehicles { get; set; }
}
public class OrderForCustomerDto : IMapFrom<Order>
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public string Number { get; set; } = null!;
    public double Quantity { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public PaymentType PaymentType { get; set; }
}
public class AddressForCustomerDto : IMapFrom<Address>
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Company { get; set; }
    public string AddressDetails1 { get; set; } = null!;
    public string? AddressDetails2 { get; set; }
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Postcode { get; set; } = null!;
    public bool Default { get; set; }
}