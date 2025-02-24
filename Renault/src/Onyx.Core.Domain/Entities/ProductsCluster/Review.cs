using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Domain.Entities.ProductsCluster;
public class Review : BaseAuditableEntity
{
    public Review()
    {
        Date = DateTime.Now;
    }
    /// <summary>
    /// تاریخ ثبت
    /// </summary>
    public DateTime Date { get; }
    /// <summary>
    /// امتیازدهی
    /// </summary>
    public int Rating { get; set; }
    /// <summary>
    /// محتوا
    /// </summary>
    public string Content { get; set; } = null!;
    /// <summary>
    /// نام مولف
    /// </summary>
    public string AuthorName { get; set; } = null!;
    /// <summary>
    /// محصول مرتبط
    /// </summary>
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }
    /// <summary>
    /// نویسنده نظر
    /// </summary>
    public Customer Customer { get; set; } = null!;
    public Guid CustomerId { get; set; }
}