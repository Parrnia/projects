
namespace Onyx.Domain.Entities.ReturnOrdersCluster;

public class ReturnOrderItemDocument : BaseAuditableEntity
{
    /// <summary>
    /// تصویر
    /// </summary>
    public Guid Image { get; set; }
    /// <summary>
    /// توضیحات
    /// </summary>
    public string Description { get; set; } = null!;
    public ReturnOrderItem ReturnOrderItem { get; set; } = null!;
    public int ReturnOrderItemId { get; set; }
}