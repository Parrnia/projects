using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice;
using Onyx.Application.Main.UserProfilesCluster.Users.Queries.BackOffice;
using Onyx.Domain.Entities.BlogsCluster;

namespace Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice;
public class PostDto : IMapFrom<Post>
{
    public PostDto()
    {
        Comments = new List<CommentDto>();
    }
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string? Image { get; set; }
    public DateTime Date { get; set; }
    public UserDto Author { get; set; } = null!;
    public IList<CommentDto> Comments { get; set; }
    public bool IsActive { get; set; }
}
