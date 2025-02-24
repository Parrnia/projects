using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.FrontOffice;
public class BlogCategoryDto : IMapFrom<BlogCategory>
{
    public BlogCategoryDto()
    {
        BlogChildrenCategories = new List<BlogChildCategoryDto>();
        BlogCategoryCustomFields = new List<BlogCategoryCustomFieldDto>();
    }
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public Guid Image { get; set; }
    
    public CategoryTypeEnum CategoryType { get; set; }
    public BlogParentCategoryDto? BlogParentCategory { get; set; } = null!;
    public List<BlogChildCategoryDto>? BlogChildrenCategories { get; set; }
    public List<BlogCategoryCustomFieldDto> BlogCategoryCustomFields { get; set; }
}
public class BlogParentCategoryDto : IMapFrom<BlogCategory>
{
    public BlogParentCategoryDto()
    {
        BlogCategoryCustomFields = new List<BlogCategoryCustomFieldDto>();
    }
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public Guid Image { get; set; }
    
    public CategoryTypeEnum CategoryType { get; set; }
    public BlogParentCategoryDto? BlogParentCategory { get; set; } = null!;
    public List<BlogCategoryCustomFieldDto> BlogCategoryCustomFields { get; set; }
}
public class BlogChildCategoryDto : IMapFrom<BlogCategory>
{
    public BlogChildCategoryDto()
    {
        BlogChildrenCategories = new List<BlogChildCategoryDto>();
        BlogCategoryCustomFields = new List<BlogCategoryCustomFieldDto>();
    }
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public Guid Image { get; set; }
    public bool IsActive { get; set; }
    public CategoryTypeEnum CategoryType { get; set; }
    public List<BlogChildCategoryDto>? BlogChildrenCategories { get; set; }
    public List<BlogCategoryCustomFieldDto> BlogCategoryCustomFields { get; set; }
}
