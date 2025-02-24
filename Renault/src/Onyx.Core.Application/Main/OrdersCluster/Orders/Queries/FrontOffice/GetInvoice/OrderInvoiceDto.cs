using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetInvoice;
public class OrderInvoiceDto : IMapFrom<Order>
{
    public int Id { get; set; }
    public string CustomerFirstName { get; set; } = null!;
    public string CustomerLastName { get; set; } = null!;
    public string NationalCode { get; set; } = null!;
    public OrderAddressDto OrderAddress { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public PersonType PersonType { get; set; }
    public double Quantity { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public PaymentType PaymentType { get; set; }
    public List<OrderInvoiceItemDto> Items { get; set; } = new List<OrderInvoiceItemDto>();
    public List<OrderInvoiceTotalDto> Totals { get; set; } = new List<OrderInvoiceTotalDto>();
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    public Guid CustomerId { get; set; }
    public string Token { get; set; } = null!;
    public double TaxPercent { get; set; }


    public string? OrderInvoiceNumber { get; set; } = null!;
    public string? OrderInvoiceSerial { get; set; } = null!;
    public string? OrderInvoiceDate { get; set; }
    public string? OrderInvoiceTime { get; set; }
    public string? PreOrderInvoiceNumber { get; set; }
    public string? SaleNumber { get; set; } = null!;
}