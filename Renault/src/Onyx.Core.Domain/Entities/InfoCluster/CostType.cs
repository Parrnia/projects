namespace Onyx.Domain.Entities.InfoCluster;

public class CostType : BaseAuditableEntity
{
    /// <summary>
    /// مقدار
    /// </summary>
    public string Value { get; set; } = null!;
    /// <summary>
    /// متن
    /// </summary>
    public string Text { get; set; } = null!;
    /// <summary>
    /// نوع هزینه
    /// </summary>
    public CostTypeEnum CostTypeEnum { get; set; }
}