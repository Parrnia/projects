using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrder.GetOrderForReturnById;

public class OrderItemForReturnByIdDto : IMapFrom<OrderItem>
{
    public OrderItemForReturnByIdDto()
    {
        Options = new List<OrderItemOptionDto>();
    }

    public int Id { get; set; }
    public decimal Price { get; set; }
    public double TotalDiscountPercent { get; set; }
    public double TaxPercent { get; set; }
    public double Quantity { get; set; }
    public decimal Total { get; set; }
    public string ProductLocalizedName { get; set; } = null!;
    public string? ProductNo { get; set; }

    public List<OrderItemProductAttributeOptionValueDto> OptionValues { get; set; } =
        new List<OrderItemProductAttributeOptionValueDto>();

    public ProductAttributeOptionForReturnByIdDto ProductAttributeOption { get; set; } = null!;
    public int ProductAttributeOptionId { get; set; }
    public List<OrderItemOptionDto> Options { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<OrderItem, OrderItemForReturnByIdDto>()
            .ForMember(i => i.TotalDiscountPercent, opt =>
                opt.MapFrom(s => s.DiscountPercentForProduct + s.Order.DiscountPercentForRole))
            .ForMember(i => i.TaxPercent, opt =>
                opt.MapFrom(s => s.Order.TaxPercent));
    }
}