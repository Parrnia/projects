namespace Onyx.Application.Main.UserProfilesCluster.Credits.Queries.GetCreditsWithPagination;
public class FilteredCreditDto
{
    public List<CreditDto> Credits { get; set; } = new List<CreditDto>();
    public int Count { get; set; }
}
