using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProduct.GetProductBySlug;
public class ProductBySlugDto : IMapFrom<Product>
{
    public ProductBySlugDto()
    {
        Images = new List<ProductImageBySlugDto>();
        Tags = new List<TagBySlugDto>();
        Attributes = new List<ProductAttributeBySlugDto>();
        AttributeOptions = new List<ProductAttributeOptionBySlugDto>();
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
    public IList<ProductImageBySlugDto> Images { get; set; }
    public int ReviewsLength { get; set; }
    public IList<int> KindIds { get; set; }
    public CompatibilityEnum Compatibility { get; set; }
    public BrandBySlugDto ProductBrand { get; set; } = null!;
    public IList<TagBySlugDto> Tags { get; set; }
    public ProductAttributeTypeBySlugDto ProductAttributeType { get; set; } = null!;
    public ProductCategoryDto ProductCategory { get; set; } = null!;
    public IList<ProductAttributeBySlugDto> Attributes { get; set; }
    public ProductOptionColorBySlugDto? ColorOption { get; set; }
    public ProductOptionMaterialBySlugDto? MaterialOption { get; set; }
    public int Rating { get; set; }
    public IList<ProductAttributeOptionBySlugDto> AttributeOptions { get; set; }
    public IList<ProductCustomFieldDto> ProductCustomFields { get; set; }

    public double OrderRate { get; set; }

    public ProductAttributeOptionBySlugDto? SelectedProductAttributeOption { get; set; }

    public string CountryName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductBySlugDto>()
            .ForMember(d => d.KindIds, opt => opt.MapFrom(s => s.Kinds.Select(x => x.Id)))
            .ForMember(d => d.ReviewsLength, opt => opt.MapFrom(s => s.Reviews.Count == 0 ? 0 : s.Reviews.Count(c => c.IsActive)))
            .ForMember(d => d.Rating, opt => opt.MapFrom(s => s.Reviews.Count == 0 ? 0 : s.Reviews.Sum(c => c.Rating) / s.Reviews.Count))
            .ForMember(d => d.CountryName,
                opt => opt.MapFrom(s => s.Country != null ? s.Country.LocalizedName : ""))
            .ForMember(d => d.Compatibility, opt => opt.MapFrom(s => s.Compatibility == CompatibilityEnum.All ? CompatibilityEnum.All : (s.Kinds.Count != 0 ? CompatibilityEnum.Compatible : CompatibilityEnum.Unknown)));

    }

}
public class ProductImageBySlugDto : IMapFrom<ProductImage>
{
    public int Id { get; set; }
    public Guid Image { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
public class BrandBySlugDto : IMapFrom<ProductBrand>
{
    public int Id { get; set; }
    public Guid? BrandLogo { get; set; } = null!;
    public string? LocalizedName { get; set; }
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public string? CountryName { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductBrand, BrandBySlugDto>()
            .ForMember(d => d.CountryName, opt => opt.MapFrom(s => s.Country != null ? s.Country.LocalizedName : null));
    }
}
public class TagBySlugDto : IMapFrom<Tag>
{
    public int Id { get; set; }
    public string EnTitle { get; set; } = null!;
    public string FaTitle { get; set; } = null!;
    public bool IsActive { get; set; }
}
public class ProductAttributeTypeBySlugDto : IMapFrom<ProductAttributeType>
{
    public ProductAttributeTypeBySlugDto()
    {
        AttributeGroups = new List<ProductTypeAttributeGroupBySlugDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public IList<ProductTypeAttributeGroupBySlugDto> AttributeGroups { get; set; }
}
public class ProductTypeAttributeGroupBySlugDto : IMapFrom<ProductTypeAttributeGroup>
{
    public ProductTypeAttributeGroupBySlugDto()
    {
        ProductTypeAttributeGroupCustomFields = new List<ProductTypeAttributeGroupCustomFieldDto>();
        Attributes = new List<ProductTypeAttributeGroupAttributeBySlugDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public List<ProductTypeAttributeGroupCustomFieldDto> ProductTypeAttributeGroupCustomFields { get; set; }
    public List<ProductTypeAttributeGroupAttributeBySlugDto> Attributes { get; set; }
}
public class ProductTypeAttributeGroupAttributeBySlugDto : IMapFrom<ProductTypeAttributeGroupAttribute>
{
    public int Id { get; set; }
    public string Value { get; set; } = null!;
}
public class ProductAttributeBySlugDto : IMapFrom<ProductAttribute>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public bool Featured { get; set; }
    public string ValueName { get; set; } = null!;
    public string ValueSlug { get; set; } = null!;
}
public class ProductOptionColorBySlugDto : IMapFrom<ProductOptionColor>
{
    public ProductOptionColorBySlugDto()
    {
        Values = new List<ProductOptionValueColorBySlugDto>();
        ProductOptionColorCustomFields = new List<ProductOptionColorCustomFieldDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ProductOptionTypeEnum Type { get; set; }
    public List<ProductOptionValueColorBySlugDto> Values { get; set; }
    public List<ProductOptionColorCustomFieldDto> ProductOptionColorCustomFields { get; set; }
}
public class ProductOptionValueColorBySlugDto : IMapFrom<ProductOptionValueColor>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Color { get; set; } = null!;
}
public class ProductOptionMaterialBySlugDto : IMapFrom<ProductOptionMaterial>
{
    public ProductOptionMaterialBySlugDto()
    {
        Values = new List<ProductOptionValueMaterialBySlugDto>();
        ProductOptionMaterialCustomFields = new List<ProductOptionMaterialCustomFieldDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ProductOptionTypeEnum Type { get; set; }
    public List<ProductOptionValueMaterialBySlugDto> Values { get; set; }
    public List<ProductOptionMaterialCustomFieldDto> ProductOptionMaterialCustomFields { get; set; }
}
public class ProductOptionValueMaterialBySlugDto : IMapFrom<ProductOptionValueMaterial>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
}
public class ProductAttributeOptionBySlugDto : IMapFrom<ProductAttributeOption>
{
    public int Id { get; set; }
    public List<ProductAttributeOptionValueBySlugDto> OptionValues { get; set; } = new List<ProductAttributeOptionValueBySlugDto>();
    public int ProductId { get; set; }
    public List<ProductAttributeOptionRoleBySlugDto> ProductAttributeOptionRoles { get; set; } = new List<ProductAttributeOptionRoleBySlugDto>();
    public bool IsDefault { get; set; }
    public List<PriceDto> Prices { get; set; } = new List<PriceDto>();
    public List<BadgeDto> Badges { get; set; } = new List<BadgeDto>();
}
public class ProductAttributeOptionValueBySlugDto : IMapFrom<ProductAttributeOptionValue>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}
public class ProductAttributeOptionRoleBySlugDto : IMapFrom<ProductAttributeOptionRole>
{
    public int Id { get; set; }
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    public double DiscountPercent { get; set; }
    public AvailabilityEnum Availability { get; set; }
    public double CurrentMaxOrderQty { get; set; }
    public double CurrentMinOrderQty { get; set; }
}