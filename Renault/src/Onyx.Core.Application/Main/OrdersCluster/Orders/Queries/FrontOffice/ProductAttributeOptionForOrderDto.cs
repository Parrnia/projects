using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice;

public class ProductAttributeOptionForOrderDto : IMapFrom<ProductAttributeOption>
{
    public int Id { get; set; }
    public string ProductName { get; set; } = null!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductAttributeOption, ProductAttributeOptionForOrderDto>()
            .ForMember(d => d.ProductName,
                opt =>
                    opt.MapFrom(s => s.Product.LocalizedName));
    }
}