using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BlogsCluster;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.BlogsCluster.WidgetComments.Queries.BackOffice;
public class WidgetCommentDto : IMapFrom<WidgetComment>
{
    public int Id { get; set; }
    public string? PostTitle { get; set; }
    public string? Text { get; set; }
    public DateTime Date { get; set; }
    public CustomerDto Author { get; set; } = null!;
    public bool IsActive { get; set; }
}
public class CustomerDto : IMapFrom<Customer>
{
    public Guid Id { get; set; }
    public string? Company { get; set; }
    public byte[]? Avatar { get; set; }
}