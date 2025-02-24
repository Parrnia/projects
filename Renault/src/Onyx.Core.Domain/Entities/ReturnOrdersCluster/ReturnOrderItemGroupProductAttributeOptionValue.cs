namespace Onyx.Domain.Entities.ReturnOrdersCluster;
public class ReturnOrderItemGroupProductAttributeOptionValue : BaseAuditableEntity
{
    /// <summary>
    /// نام گزینه ساختاری
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// مقدار ویژگی محصول
    /// </summary>
    public string Value { get; set; } = null!;
    public ReturnOrderItemGroup ReturnOrderItemGroup { get; set; } = null!;
    public int ReturnOrderItemGroupId { get; set; }
}
