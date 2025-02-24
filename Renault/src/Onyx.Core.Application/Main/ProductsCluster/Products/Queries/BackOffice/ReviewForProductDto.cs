using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice;
public class ReviewForProductDto : IMapFrom<Review>
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; } = null!;
    public bool IsActive { get; set; }
}
