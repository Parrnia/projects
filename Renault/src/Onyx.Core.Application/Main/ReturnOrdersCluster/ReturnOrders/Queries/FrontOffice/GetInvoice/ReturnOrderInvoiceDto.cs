using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetInvoice;
public class ReturnOrderInvoiceDto : IMapFrom<ReturnOrder>
{
    public int Id { get; set; }
    public string CustomerFirstName { get; set; } = null!;
    public string CustomerLastName { get; set; } = null!;
    public string NationalCode { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Number { get; set; } = null!;
    public double Quantity { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public CostRefundType CostRefundType { get; set; }
    public ReturnOrderTransportationType ReturnOrderTransportationType { get; set; }
    public List<ReturnOrderInvoiceItemGroupDto> ItemGroups { get; set; } = new List<ReturnOrderInvoiceItemGroupDto>();
    public List<ReturnOrderInvoiceTotalDto> Totals { get; set; } = new List<ReturnOrderInvoiceTotalDto>();
    public Guid CustomerId { get; set; }
    public string Token { get; set; } = null!;
    public string OrderNumber { get; set; } = null!;

    public string? ReturnOrderInvoiceNumber { get; set; } = null!;
    public string? ReturnOrderInvoiceSerial { get; set; } = null!;
    public string? ReturnOrderInvoiceDate { get; set; }
    public string? ReturnOrderInvoiceTime { get; set; }
    public string? PreReturnOrderInvoiceNumber { get; set; }
    public string SaleNumber { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ReturnOrder, ReturnOrderInvoiceDto>()
            .ForMember(d => d.CustomerFirstName,
                opt =>
                    opt.MapFrom(s => s.Order.CustomerFirstName))
            .ForMember(d => d.CustomerLastName,
                opt =>
                    opt.MapFrom(s => s.Order.CustomerLastName))
            .ForMember(d => d.NationalCode,
                opt =>
                    opt.MapFrom(s => s.Order.NationalCode))
            .ForMember(d => d.PhoneNumber,
                opt =>
                    opt.MapFrom(s => s.Order.PhoneNumber))
            .ForMember(d => d.CustomerId,
                opt =>
                    opt.MapFrom(s => s.Order.CustomerId))
            .ForMember(d => d.OrderNumber,
                opt =>
                    opt.MapFrom(s => s.Order.Number))
            ;
    }
}