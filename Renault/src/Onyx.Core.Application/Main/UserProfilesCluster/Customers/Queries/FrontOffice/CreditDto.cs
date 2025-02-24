using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Queries.FrontOffice;
public class CreditDto : IMapFrom<Credit>
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Value { get; set; }
    public string ModifierUserName { get; set; } = null!;
    public string? OrderToken { get; set; }
}