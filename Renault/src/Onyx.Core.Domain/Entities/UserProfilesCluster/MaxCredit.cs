namespace Onyx.Domain.Entities.UserProfilesCluster;

public class MaxCredit : BaseAuditableEntity
{
    /// <summary>
    /// تاریخ ثبت
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// مقدار اعتبار
    /// </summary>
    public decimal Value { get; set; }
    /// <summary>
    /// نام کاربر تغییردهنده 
    /// </summary>
    public string ModifierUserName { get; set; } = null!;
    /// <summary>
    /// شناسه کاربر تغییردهنده
    /// </summary>
    public Guid ModifierUserId { get; set; }
    /// <summary>
    /// مشتری مرتبط
    /// </summary>
    public Customer Customer { get; set; } = null!;
    public Guid CustomerId { get; set; }
}