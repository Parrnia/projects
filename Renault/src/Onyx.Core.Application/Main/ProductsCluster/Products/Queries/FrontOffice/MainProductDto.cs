using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice;

public class MainProductDto : IMapFrom<Product>
{
    public int Id { get; set; }
    public Guid? Related7SoftProductId { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Excerpt { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Sku { get; set; }
    public string? ProductNo { get; set; }
    public IList<MainProductImageDto> Images { get; set; } = new List<MainProductImageDto>();
    public int ReviewsLength { get; set; }
    public IList<int> KindIds { get; set; } = new List<int>();
    public CompatibilityEnum Compatibility { get; set; }
    public MainBrandDto ProductBrand { get; set; } = null!;
    public IList<MainTagDto> Tags { get; set; } = new List<MainTagDto>();
    public MainProductAttributeTypeDto ProductAttributeType { get; set; } = null!;
    public ProductCategoryDto ProductCategory { get; set; } = null!;
    public IList<MainProductAttributeDto> Attributes { get; set; } = new List<MainProductAttributeDto>();
    public MainProductOptionColorDto? ColorOption { get; set; }
    public MainProductOptionMaterialDto? MaterialOption { get; set; }
    public int Rating { get; set; }
    public IList<MainProductAttributeOptionDto> AttributeOptions { get; set; } = new List<MainProductAttributeOptionDto>();
    public IList<ProductCustomFieldDto> ProductCustomFields { get; set; } = new List<ProductCustomFieldDto>();

    public double OrderRate { get; set; }

    public MainProductAttributeOptionDto? SelectedProductAttributeOption { get; set; }

    public string CountryName { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, MainProductDto>()
            .ForMember(d => d.KindIds, opt => opt.MapFrom(s => s.Kinds.Select(x => x.Id)))
            .ForMember(d => d.ReviewsLength, opt => opt.MapFrom(s => s.Reviews.Count == 0 ? 0 : s.Reviews.Count(c => c.IsActive)))
            .ForMember(d => d.Rating, opt => opt.MapFrom(s => s.Reviews.Count == 0 ? 0 : s.Reviews.Sum(c => c.Rating) / s.Reviews.Count))
            .ForMember(d => d.CountryName,
                opt => opt.MapFrom(s => s.Country != null ? s.Country.LocalizedName : ""))
            .ForMember(d => d.Compatibility, opt => opt.MapFrom(s => s.Compatibility == CompatibilityEnum.All ? CompatibilityEnum.All : (s.Kinds.Count != 0 ? CompatibilityEnum.Compatible : CompatibilityEnum.Unknown)));
    }

}
public class MainProductImageDto : IMapFrom<ProductImage>
{
    public int Id { get; set; }
    public Guid Image { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
public class MainBrandDto : IMapFrom<ProductBrand>
{
    public int Id { get; set; }
    public Guid? BrandLogo { get; set; } = null!;
    public string? LocalizedName { get; set; }
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public string? CountryName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductBrand, MainBrandDto>()
            .ForMember(d => d.CountryName, opt => opt.MapFrom(s => s.Country != null ? s.Country.LocalizedName : null));
    }
}
public class MainTagDto : IMapFrom<Tag>
{
    public int Id { get; set; }
    public string EnTitle { get; set; } = null!;
    public string FaTitle { get; set; } = null!;
    public bool IsActive { get; set; }
}
public class MainProductAttributeTypeDto : IMapFrom<ProductAttributeType>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public IList<MainProductTypeAttributeGroupDto> AttributeGroups { get; set; } = new List<MainProductTypeAttributeGroupDto>();
}
public class MainProductTypeAttributeGroupDto : IMapFrom<ProductTypeAttributeGroup>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public List<ProductTypeAttributeGroupCustomFieldDto> ProductTypeAttributeGroupCustomFields { get; set; } = new List<ProductTypeAttributeGroupCustomFieldDto>();
    public List<MainProductTypeAttributeGroupAttributeDto> Attributes { get; set; } = new List<MainProductTypeAttributeGroupAttributeDto>();
}
public class MainProductTypeAttributeGroupAttributeDto : IMapFrom<ProductTypeAttributeGroupAttribute>
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
}
public class MainProductAttributeDto : IMapFrom<ProductAttribute>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public bool Featured { get; set; }
    public string ValueName { get; set; } = null!;
    public string ValueSlug { get; set; } = null!;
}
public class MainProductOptionColorDto : IMapFrom<ProductOptionColor>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ProductOptionTypeEnum Type { get; set; }
    public List<MainProductOptionValueColorDto> Values { get; set; } = new List<MainProductOptionValueColorDto>();
    public List<ProductOptionColorCustomFieldDto> ProductOptionColorCustomFields { get; set; } = new List<ProductOptionColorCustomFieldDto>();
}
public class MainProductOptionValueColorDto : IMapFrom<ProductOptionValueColor>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Color { get; set; } = null!;
}
public class MainProductOptionMaterialDto : IMapFrom<ProductOptionMaterial>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ProductOptionTypeEnum Type { get; set; }
    public List<MainProductOptionValueMaterialDto> Values { get; set; } = new List<MainProductOptionValueMaterialDto>();
    public List<ProductOptionMaterialCustomFieldDto> ProductOptionMaterialCustomFields { get; set; } = new List<ProductOptionMaterialCustomFieldDto>();
}
public class MainProductOptionValueMaterialDto : IMapFrom<ProductOptionValueMaterial>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
}
public class MainProductAttributeOptionDto : IMapFrom<ProductAttributeOption>
{
    public int Id { get; set; }
    public List<MainProductAttributeOptionValueDto> OptionValues { get; set; } = new List<MainProductAttributeOptionValueDto>();
    public int ProductId { get; set; }
    public List<MainProductAttributeOptionRoleDto> ProductAttributeOptionRoles { get; set; } = new List<MainProductAttributeOptionRoleDto>();
    public bool IsDefault { get; set; }
    public List<PriceDto> Prices { get; set; } = new List<PriceDto>();
    public List<BadgeDto> Badges { get; set; } = new List<BadgeDto>();
}
public class MainProductAttributeOptionValueDto : IMapFrom<ProductAttributeOptionValue>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}
public class MainProductAttributeOptionRoleDto : IMapFrom<ProductAttributeOptionRole>
{
    public int Id { get; set; }
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    public double DiscountPercent { get; set; }
    public AvailabilityEnum Availability { get; set; }
    public double CurrentMaxOrderQty { get; set; }
    public double CurrentMinOrderQty { get; set; }
}