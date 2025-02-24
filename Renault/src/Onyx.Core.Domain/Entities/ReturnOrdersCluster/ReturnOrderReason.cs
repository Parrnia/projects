namespace Onyx.Domain.Entities.ReturnOrdersCluster;

public class ReturnOrderReason : BaseAuditableEntity
{
    /// <summary>
    /// توضیحات
    /// </summary>
    public string Details { get; set; } = null!;
    /// <summary>
    /// نوع کلی
    /// </summary>
    public ReturnOrderReasonType ReturnOrderReasonType { get; private set; }
    /// <summary>
    /// نوع دلیل سمت مشتری
    /// </summary>
    public ReturnOrderCustomerReasonType? CustomerType { get; private set; }
    /// <summary>
    /// نوع دلیل سمت سازمان
    /// </summary>
    public ReturnOrderOrganizationReasonType? OrganizationType { get; private set; }
    public ReturnOrderItem ReturnOrderItem { get; set; } = default!;
    public int ReturnOrderItemId { get; set; }

    public void SetReturnOrderReasonType(ReturnOrderCustomerReasonType? returnOrderCustomerReasonType, ReturnOrderOrganizationReasonType? returnOrderOrganizationReasonType)
    {
        if (returnOrderCustomerReasonType != null)
        {
            ReturnOrderReasonType = ReturnOrderReasonType.CustomerSide;
            CustomerType = returnOrderCustomerReasonType;
            OrganizationType = null;
        } else
        {
            ReturnOrderReasonType = ReturnOrderReasonType.OrganizationSide;
            OrganizationType = returnOrderOrganizationReasonType;    
            CustomerType = null;
        }
    }
}