using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Domain.Entities.BlogsCluster;
public class Comment : BaseAuditableEntity
{
    public Comment()
    {
        Date = DateTime.Now;
    }
    /// <summary>
    /// متن
    /// </summary>
    public string Text { get; set; } = null!;
    /// <summary>
    /// تاریخ ثبت
    /// </summary>
    public DateTime Date { get; }
    /////// <summary>
    /////// پاسخ ها
    /////// </summary>
    //public List<Comment> Children { get; set; } = new List<Comment>();
    /// <summary>
    /// نظر دهنده
    /// </summary>
    public Customer Author { get; set; } = null!;
    public Guid AuthorId { get; set; }
    /// <summary>
    /// پست مربوط به نظر
    /// </summary>
    public Post Post { get; set; } = null!;
    public int PostId { get; set; }
    ///// <summary>
    ///// نظر مادر
    ///// </summary>
    //public Comment? ParentComment { get; set; }
    //public int? ParentCommentId { get; set; }
}