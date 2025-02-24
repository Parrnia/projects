using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice;
public class ProductCategoryForProductDto : IMapFrom<ProductCategory>
{
    public ProductCategoryForProductDto()
    {
        ProductSubCategoryCustomFields = new List<ProductCategoryCustomFieldDto>();
    }
    public int Id { get; set; }
    public int Code { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public string? ProductSubCategoryNo { get; set; }
    public Guid? Image { get; set; }
    
    public CategoryTypeEnum CategoryType { get; set; }
    public List<ProductCategoryCustomFieldDto> ProductSubCategoryCustomFields { get; set; }
}
