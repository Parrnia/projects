using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Queries.BackOffice;
public class CustomerDto : IMapFrom<Customer>
{
    public Guid Id { get; set; }
    public Guid? Avatar { get; set; }
    public List<CreditDto> Credits { get; set; } = new List<CreditDto>();
    public List<MaxCreditDto> MaxCredits { get; set; } = new List<MaxCreditDto>();
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