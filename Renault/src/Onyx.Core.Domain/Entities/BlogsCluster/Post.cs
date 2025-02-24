using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Domain.Entities.BlogsCluster;
public class Post : BaseAuditableEntity
{
    public Post()
    {
        Date = DateTime.Now;
    }
    /// <summary>
    /// عنوان
    /// </summary>
    public string Title { get; set; } = null!;
    /// <summary>
    /// متن
    /// </summary>
    public string Body { get; set; } = null!;
    /// <summary>
    /// تصویر
    /// </summary>
    public byte[]? Image { get; set; }
    /// <summary>
    /// تاریخ انتشار
    /// </summary>
    public DateTime Date { get; }
    /// <summary>
    /// زیر دسته بندی بلاگ
    /// </summary>
    public BlogCategory BlogCategory { get; set; } = null!;
    public int BlogCategoryId { get; set; }
    /// <summary>
    /// مولف پست
    /// </summary>
    public User Author { get; set; } = null!;
    public Guid AuthorId { get; set; }
    /// <summary>
    /// نظرات پست
    /// </summary>
    public List<Comment> Comments { get; set; } = new List<Comment>();
}