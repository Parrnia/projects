using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.CategoriesCluster;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.BackOffice;
public class BlogCategoryCustomFieldDto : IMapFrom<BlogCategoryCustomField>
{
    public string FieldName { get; set; } = null!;
    public string Value { get; set; } = null!;
}
