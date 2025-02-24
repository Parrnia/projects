using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.BackOffice;
public class AllProductTypeAttributeGroupAttributeDto : IMapFrom<ProductTypeAttributeGroupAttribute>
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
    public int ProductTypeAttributeGroupId { get; set; }
    public string ProductTypeAttributeGroupName { get; set; } = null!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductTypeAttributeGroupAttribute, AllProductTypeAttributeGroupAttributeDto>()
            .ForMember(d => d.ProductTypeAttributeGroupId, opt => opt.MapFrom(s => s.ProductTypeAttributeGroupId))
            .ForMember(d => d.ProductTypeAttributeGroupName, opt => opt.MapFrom(s => s.ProductTypeAttributeGroup.Name));
    }
}
