using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.FrontOffice.GetReviews.GetReviewsByCustomerId;
public class ReviewByCustomerIdDto : IMapFrom<Review>
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public CustomerDto Customer { get; set; } = null!;
}
public class CustomerDto : IMapFrom<Customer>
{
    public Guid Id { get; set; }
    public string? Company { get; set; }
    public Guid? Avatar { get; set; } = null!;
}
