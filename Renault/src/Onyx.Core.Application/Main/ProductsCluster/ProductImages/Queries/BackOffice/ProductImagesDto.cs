using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductImages.Queries.BackOffice;

public class ProductImagesDto : IMapFrom<ProductImage>
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Guid Image { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
