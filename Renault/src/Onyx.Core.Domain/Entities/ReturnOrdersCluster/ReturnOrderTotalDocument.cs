
namespace Onyx.Domain.Entities.ReturnOrdersCluster;

public class ReturnOrderTotalDocument : BaseAuditableEntity
{
    /// <summary>
    /// تصویر
    /// </summary>
    public Guid Image { get; set; }
    /// <summary>
    /// توضیحات
    /// </summary>
    public string Description { get; set; } = null!;
    public ReturnOrderTotal ReturnOrderTotal { get; set; } = null!;
    public int ReturnOrderTotalId { get; set; }
}