using Onyx.Domain.Interfaces;

namespace Onyx.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity,IChangeDateAware
{
    /// <summary>
    /// تاریخ ایجاد
    /// </summary>
    public DateTime Created { get; set; }
    /// <summary>
    /// کاربر ایجادکننده
    /// </summary>
    public string? CreatedBy { get; set; }
    /// <summary>
    /// تاریخ آخرین تغییر
    /// </summary>
    public DateTime? LastModified { get; set; }
    /// <summary>
    /// آخرین کاربر تغییردهنده
    /// </summary>
    public string? LastModifiedBy { get; set; }
    /// <summary>
    /// حذف شده
    /// </summary>
    public bool? IsDeleted { get; set; }
    /// <summary>
    /// وضعیت
    /// </summary>
    public bool IsActive { get; set; }
}
