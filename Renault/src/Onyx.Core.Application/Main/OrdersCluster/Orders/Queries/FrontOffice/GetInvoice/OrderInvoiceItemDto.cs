using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetInvoice;

public class OrderInvoiceItemDto : IMapFrom<OrderItem>
{
    public string? ProductNo { get; set; }
    public string ProductLocalizedName { get; set; } = null!;
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
    public double TotalDiscountPercent { get; set; }
    public decimal TaxPrice { get; set; }
    public List<OrderItemProductAttributeOptionValueDto> OptionValues { get; set; } = new List<OrderItemProductAttributeOptionValueDto>();
    public void Mapping(Profile profile)
    {
        profile.CreateMap<OrderItem, OrderInvoiceItemDto>()
            .ForMember(d => d.TotalDiscountPercent,
                opt => opt.MapFrom(s => s.DiscountPercentForProduct + s.Order.DiscountPercentForRole));
    }
}