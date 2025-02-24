using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Domain.Entities.BlogsCluster;

public class WidgetComment : BaseAuditableEntity
{
    public WidgetComment()
    {
        Date = DateTime.Now;
    }
    /// <summary>
    /// عنوان پست
    /// </summary>
    public string? PostTitle { get; set; }
    /// <summary>
    /// متن
    /// </summary>
    public string? Text { get; set; }
    /// <summary>
    /// تاریخ ثبت
    /// </summary>
    public DateTime Date { get; }
    /// <summary>
    /// نظر دهنده
    /// </summary>
    public Customer Author { get; set; } = null!;
    public Guid AuthorId { get; set; }
}