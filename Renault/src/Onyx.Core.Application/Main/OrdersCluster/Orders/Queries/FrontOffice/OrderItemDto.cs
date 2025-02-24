using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice;

public class OrderItemDto : IMapFrom<OrderItem>
{
    public OrderItemDto()
    {
        Options = new List<OrderItemOptionDto>();
    }
    public int Id { get; set; }
    public decimal Price { get; set; }
    public double DiscountPercentForProduct { get; set; }
    public double Quantity { get; set; }
    public decimal Total { get; set; }
    public string ProductLocalizedName { get; set; } = null!;
    public string ProductSlug { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string? ProductNo { get; set; }
    public List<OrderItemProductAttributeOptionValueDto> OptionValues { get; set; } = new List<OrderItemProductAttributeOptionValueDto>();
    public ProductAttributeOptionForOrderDto ProductAttributeOption { get; set; } = null!;
    public int ProductAttributeOptionId { get; set; }
    public List<OrderItemOptionDto> Options { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductSlug,
                opt =>
                    opt.MapFrom(s => s.ProductAttributeOption.Product.Slug));
    }
}