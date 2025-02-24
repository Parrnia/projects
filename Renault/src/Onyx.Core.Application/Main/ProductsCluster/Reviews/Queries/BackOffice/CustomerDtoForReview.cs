using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice;
public class CustomerDtoForReview : IMapFrom<Customer>
{
    public Guid Id { get; set; }
    public string? Company { get; set; }
    public string? Avatar { get; set; } = null!;
}
