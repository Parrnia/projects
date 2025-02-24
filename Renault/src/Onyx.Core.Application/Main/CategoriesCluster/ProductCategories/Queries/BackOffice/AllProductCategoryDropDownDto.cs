using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.CategoriesCluster;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.BackOffice;
public class AllProductCategoryDropDownDto : IMapFrom<ProductCategory>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
}

