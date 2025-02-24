using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategoriesWithPagination.GetFeaturedFirstProductCategoriesWithPagination;
public class FeaturedFirstProductCategoryWithPaginationDto : IMapFrom<ProductCategory>
{
    public FeaturedFirstProductCategoryWithPaginationDto()
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
    
    public CategoryTypeEnum CategoryType { get; set; }
    public ProductParentCategoryDto? ProductParentCategory { get; set; } = null!;
    public List<ProductChildCategoryDto>? ProductChildrenCategories { get; set; }
    public List<ProductCategoryCustomFieldDto> ProductCategoryCustomFields { get; set; }
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
    public bool IsActive { get; set; }

    public CategoryTypeEnum CategoryType { get; set; }
    public List<ProductChildCategoryDto>? ProductChildrenCategories { get; set; }
    public List<ProductCategoryCustomFieldDto> ProductCategoryCustomFields { get; set; }
}

