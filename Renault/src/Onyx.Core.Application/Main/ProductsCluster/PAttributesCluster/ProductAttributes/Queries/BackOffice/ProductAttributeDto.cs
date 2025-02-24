using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.BackOffice;
public class ProductAttributeDto : IMapFrom<ProductAttribute>
{
    public ProductAttributeDto()
    {
        ProductAttributeCustomFields = new List<ProductAttributeCustomFieldDto>();
        ProductAttributeValueCustomFields = new List<ProductAttributeValueCustomFieldDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public bool Featured { get; set; }
    public IList<ProductAttributeCustomFieldDto> ProductAttributeCustomFields { get; set; }
    public string ValueName { get; set; } = null!;
    public string ValueSlug { get; set; } = null!;
    public IList<ProductAttributeValueCustomFieldDto> ProductAttributeValueCustomFields { get; set; }
}
