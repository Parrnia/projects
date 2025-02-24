using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProduct.GetProductById;
public class ProductByIdDto : IMapFrom<Product>
{
    public ProductByIdDto()
    {
        Images = new List<ProductImageByIdDto>();
        Tags = new List<TagByIdDto>();
        Attributes = new List<ProductAttributeByIdDto>();
        AttributeOptions = new List<ProductAttributeOptionByIdDto>();
        ProductCustomFields = new List<ProductCustomFieldDto>();
        KindIds = new List<int>();
    }
    public int Id { get; set; }
    public Guid? Related7SoftProductId { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Excerpt { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Sku { get; set; }
    public string? ProductNo { get; set; }
    public IList<ProductImageByIdDto> Images { get; set; }
    public int ReviewsLength { get; set; }
    public IList<int> KindIds { get; set; }
    public CompatibilityEnum Compatibility { get; set; }
    public BrandByIdDto ProductBrand { get; set; } = null!;
    public IList<TagByIdDto> Tags { get; set; }
    public ProductAttributeTypeByIdDto ProductAttributeType { get; set; } = null!;
    public ProductCategoryDto ProductCategory { get; set; } = null!;
    public IList<ProductAttributeByIdDto> Attributes { get; set; }
    public ProductOptionColorByIdDto? ColorOption { get; set; }
    public ProductOptionMaterialByIdDto? MaterialOption { get; set; }
    public int Rating { get; set; }
    public IList<ProductAttributeOptionByIdDto> AttributeOptions { get; set; }
    public IList<ProductCustomFieldDto> ProductCustomFields { get; set; }

    public double OrderRate { get; set; }

    public ProductAttributeOptionByIdDto? SelectedProductAttributeOption { get; set; }

    public string CountryName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductByIdDto>()
            .ForMember(d => d.KindIds, opt => opt.MapFrom(s => s.Kinds.Select(x => x.Id)))
            .ForMember(d => d.ReviewsLength,
                opt => opt.MapFrom(s => s.Reviews.Count == 0 ? 0 : s.Reviews.Count(c => c.IsActive)))
            .ForMember(d => d.Rating,
                opt => opt.MapFrom(s => s.Reviews.Count == 0 ? 0 : s.Reviews.Sum(c => c.Rating) / s.Reviews.Count))
            .ForMember(d => d.CountryName,
                opt => opt.MapFrom(s => s.Country != null ? s.Country.LocalizedName : ""))
            .ForMember(d => d.Compatibility, opt => opt.MapFrom(s =>  s.Compatibility == CompatibilityEnum.All ? CompatibilityEnum.All : (s.Kinds.Count != 0 ? CompatibilityEnum.Compatible : CompatibilityEnum.Unknown)));
    }

}
public class ProductImageByIdDto : IMapFrom<ProductImage>
{
    public int Id { get; set; }
    public Guid Image { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }

}
public class BrandByIdDto : IMapFrom<ProductBrand>
{
    public int Id { get; set; }
    public Guid? BrandLogo { get; set; } = null!;
    public string? LocalizedName { get; set; }
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public string? CountryName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductBrand, BrandByIdDto>()
            .ForMember(d => d.CountryName, opt => opt.MapFrom(s => s.Country != null ? s.Country.LocalizedName : null));
    }
}
public class TagByIdDto : IMapFrom<Tag>
{
    public int Id { get; set; }
    public string EnTitle { get; set; } = null!;
    public string FaTitle { get; set; } = null!;
    public bool IsActive { get; set; }
}
public class ProductAttributeTypeByIdDto : IMapFrom<ProductAttributeType>
{
    public ProductAttributeTypeByIdDto()
    {
        AttributeGroups = new List<ProductTypeAttributeGroupByIdDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public IList<ProductTypeAttributeGroupByIdDto> AttributeGroups { get; set; }
}
public class ProductTypeAttributeGroupByIdDto : IMapFrom<ProductTypeAttributeGroup>
{
    public ProductTypeAttributeGroupByIdDto()
    {
        ProductTypeAttributeGroupCustomFields = new List<ProductTypeAttributeGroupCustomFieldDto>();
        Attributes = new List<ProductTypeAttributeGroupAttributeByIdDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public List<ProductTypeAttributeGroupCustomFieldDto> ProductTypeAttributeGroupCustomFields { get; set; }
    public List<ProductTypeAttributeGroupAttributeByIdDto> Attributes { get; set; }
}
public class ProductTypeAttributeGroupAttributeByIdDto : IMapFrom<ProductTypeAttributeGroupAttribute>
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
}
public class ProductAttributeByIdDto : IMapFrom<ProductAttribute>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public bool Featured { get; set; }
    public string ValueName { get; set; } = null!;
    public string ValueSlug { get; set; } = null!;
}
public class ProductOptionColorByIdDto : IMapFrom<ProductOptionColor>
{
    public ProductOptionColorByIdDto()
    {
        Values = new List<ProductOptionValueColorByIdDto>();
        ProductOptionColorCustomFields = new List<ProductOptionColorCustomFieldDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ProductOptionTypeEnum Type { get; set; }
    public List<ProductOptionValueColorByIdDto> Values { get; set; }
    public List<ProductOptionColorCustomFieldDto> ProductOptionColorCustomFields { get; set; }
}
public class ProductOptionValueColorByIdDto : IMapFrom<ProductOptionValueColor>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Color { get; set; } = null!;
}
public class ProductOptionMaterialByIdDto : IMapFrom<ProductOptionMaterial>
{
    public ProductOptionMaterialByIdDto()
    {
        Values = new List<ProductOptionValueMaterialByIdDto>();
        ProductOptionMaterialCustomFields = new List<ProductOptionMaterialCustomFieldDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ProductOptionTypeEnum Type { get; set; }
    public List<ProductOptionValueMaterialByIdDto> Values { get; set; }
    public List<ProductOptionMaterialCustomFieldDto> ProductOptionMaterialCustomFields { get; set; }
}
public class ProductOptionValueMaterialByIdDto : IMapFrom<ProductOptionValueMaterial>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
}
public class ProductAttributeOptionByIdDto : IMapFrom<ProductAttributeOption>
{
    public int Id { get; set; }
    public List<ProductAttributeOptionValueByIdDto> OptionValues { get; set; } = new List<ProductAttributeOptionValueByIdDto>();
    public int ProductId { get; set; }
    public List<ProductAttributeOptionRoleByIdDto> ProductAttributeOptionRoles { get; set; } = new List<ProductAttributeOptionRoleByIdDto>();
    public bool IsDefault { get; set; }
    public List<PriceDto> Prices { get; set; } = new List<PriceDto>();
    public List<BadgeDto> Badges { get; set; } = new List<BadgeDto>();
}
public class ProductAttributeOptionValueByIdDto : IMapFrom<ProductAttributeOptionValue>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}
public class ProductAttributeOptionRoleByIdDto : IMapFrom<ProductAttributeOptionRole>
{
    public int Id { get; set; }
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    public double DiscountPercent { get; set; }
    public AvailabilityEnum Availability { get; set; }
    public double CurrentMaxOrderQty { get; set; }
    public double CurrentMinOrderQty { get; set; }
}