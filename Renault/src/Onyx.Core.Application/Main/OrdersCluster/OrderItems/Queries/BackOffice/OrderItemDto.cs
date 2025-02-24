using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.OrderItems.Queries.BackOffice;
public class OrderItemDto : IMapFrom<OrderItem>
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public double DiscountPercentForProduct { get; set; }
    public double Quantity { get; set; }
    public decimal Total { get; set; }
    public string ProductName { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductName,
                opt =>
                    opt.MapFrom(s => ExtractProductName(s.OptionValues.ToList(), s.ProductLocalizedName)));
    }

    private static string ExtractProductName(List<OrderItemProductAttributeOptionValue> values,string productLocalizedName)
    {
        var name = productLocalizedName;
        if (values.Count > 0)
        {
            name = values.Aggregate(name, (current, value) => current + ("/" + value.Name + ":" + value.Value));
        }
        return name;
    }
}
