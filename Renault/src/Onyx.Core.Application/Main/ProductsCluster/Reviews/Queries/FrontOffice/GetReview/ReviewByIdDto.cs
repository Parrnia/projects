using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.FrontOffice.GetReview;
public class ReviewByIdDto : IMapFrom<Review>
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public CustomerDtoForReview Customer { get; set; } = null!;
}
