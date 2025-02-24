using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BlogsCluster;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice;
public class CommentDto : IMapFrom<Comment>
{
    public CommentDto()
    {
        //Children = new List<CommentDto>();
    }
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public DateTime Date { get; set; }
    //public IList<CommentDto> Children { get; set; }
    public CustomerDto Author { get; set; } = null!;
    //public CommentDto? ParentComment { get; set; }
    public bool IsActive { get; set; }
}

public class CustomerDto : IMapFrom<Customer>
{
    public Guid Id { get; set; }
    public string? Company { get; set; }
    public byte[]? Avatar { get; set; }
}
