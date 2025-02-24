using Onyx.Application.Main.UserProfilesCluster.Customers.Queries.BackOffice;

namespace Onyx.Application.Main.UserProfilesCluster.MaxCredits.Queries.GetMaxCreditsWithPagination;
public class FilteredMaxCreditDto
{
    public List<MaxCreditDto> MaxCredits { get; set; } = new List<MaxCreditDto>();
    public int Count { get; set; }
}
