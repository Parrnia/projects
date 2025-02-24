using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Users.Queries.BackOffice;
public class UserDto : IMapFrom<User>
{
    public Guid Id { get; set; }
    public Guid? Avatar { get; set; }
}
