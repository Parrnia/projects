using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Domain.Entities.CustomerSupportCluster;
public class CustomerTicket : BaseAuditableEntity
{
    public CustomerTicket()
    {
        Date = DateTime.Now;
    }
    /// <summary>
    /// موضوع
    /// </summary>
    public string Subject { get; set; } = null!;
    /// <summary>
    /// پیام
    /// </summary>
    public string Message { get; set; } = null!;
    /// <summary>
    /// تاریخ ثبت
    /// </summary>
    public DateTime Date { get; }
    public Customer Customer { get; set; } = null!;
    public Guid CustomerId { get; set; }
    /// <summary>
    /// شماره تماس مشتری
    /// </summary>
    public string CustomerPhoneNumber { get; set; } = null!;
    /// <summary>
    /// نام مشتری
    /// </summary>
    public string CustomerName { get; set; } = null!;

}