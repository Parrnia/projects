using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.CategoriesCluster;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice;
public class ProductCategoryCustomFieldDto : IMapFrom<ProductCategoryCustomField>
{
    public string FieldName { get; set; } = null!;
    public string Value { get; set; } = null!;
}
