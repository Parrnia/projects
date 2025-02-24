using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.BackOffice;
public class ProductCategoryDto : IMapFrom<ProductCategory>
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public string? ProductCategoryNo { get; set; }
    public Guid? Image { get; set; }
    public Guid? MenuImage { get; set; }
    public int? ProductParentCategoryId { get; set; }
    public string? ProductParentCategoryName { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPopular { get; set; }
    public bool IsActive { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductCategory, ProductCategoryDto>()
            .ForMember(d => d.ProductParentCategoryId, opt => opt.MapFrom(s => s.ProductParentCategoryId))
            .ForMember(d => d.ProductParentCategoryName, opt => opt.MapFrom(s =>s.ProductParentCategory != null ? s.ProductParentCategory.LocalizedName : null));
    }
}
public class ProductParentCategoryDto : IMapFrom<ProductCategory>
{
    public ProductParentCategoryDto()
    {
        ProductCategoryCustomFields = new List<ProductCategoryCustomFieldDto>();
    }
    public int Id { get; set; }
    public int Code { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public string? ProductCategoryNo { get; set; }
    public Guid? Image { get; set; }
    public Guid? MenuImage { get; set; }
    public CategoryTypeEnum CategoryType { get; set; }
    public ProductParentCategoryDto? ProductParentCategory { get; set; } = null!;
    public List<ProductCategoryCustomFieldDto> ProductCategoryCustomFields { get; set; }
}
public class ProductChildCategoryDto : IMapFrom<ProductCategory>
{
    public ProductChildCategoryDto()
    {
        ProductChildrenCategories = new List<ProductChildCategoryDto>();
        ProductCategoryCustomFields = new List<ProductCategoryCustomFieldDto>();
    }
    public int Id { get; set; }
    public int Code { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public string? ProductCategoryNo { get; set; }
    public Guid? Image { get; set; }
    public Guid? MenuImage { get; set; }
    public CategoryTypeEnum CategoryType { get; set; }
    public List<ProductChildCategoryDto>? ProductChildrenCategories { get; set; }
    public List<ProductCategoryCustomFieldDto> ProductCategoryCustomFields { get; set; }
}

