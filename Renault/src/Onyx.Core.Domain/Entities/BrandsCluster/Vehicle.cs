using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Domain.Entities.BrandsCluster;
public class Vehicle : BaseAuditableEntity
{
    /// <summary>
    /// شماره vin
    /// </summary>
    public string? VinNumber { get; set; }
    /// <summary>
    /// نوع
    /// </summary>
    public Kind Kind { get; set; } = null!;
    public int KindId { get; set; }
    /// <summary>
    /// مشتری مرتبط
    /// </summary>
    public Customer Customer { get; set; } = null!;
    /// <summary>
    /// شناسه مشتری مرتبط
    /// </summary>
    public Guid CustomerId { get; set; }
}
