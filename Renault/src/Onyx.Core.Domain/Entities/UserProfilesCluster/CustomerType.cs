namespace Onyx.Domain.Entities.UserProfilesCluster;
public class CustomerType : BaseAuditableEntity
{
    /// <summary>
    /// نوع مشتری
    /// </summary>
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    /// <summary>
    /// درصد تخفیف
    /// </summary>
    public double DiscountPercent { get; set; }
}