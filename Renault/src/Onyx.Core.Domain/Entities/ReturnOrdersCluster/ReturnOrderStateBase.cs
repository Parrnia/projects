namespace Onyx.Domain.Entities.ReturnOrdersCluster;

public class ReturnOrderStateBase : BaseAuditableEntity
{
    public ReturnOrderStatus ReturnOrderStatus { get; set; }
    public string Details { get; set; } = null!;
    public virtual ReturnOrderStateBase Register(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual ReturnOrderStateBase Reject(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual ReturnOrderStateBase Accept(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual ReturnOrderStateBase Send(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual ReturnOrderStateBase Receive(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual ReturnOrderStateBase ConfirmAll(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual ReturnOrderStateBase RefundCost(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual ReturnOrderStateBase ConfirmSome(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual ReturnOrderStateBase Review(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual ReturnOrderStateBase Revision(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual ReturnOrderStateBase Complete(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual ReturnOrderStateBase Cancel(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public ReturnOrder ReturnOrder { get; set; } = null!;
    public int ReturnOrderId { get; set; }
}
public class PendingForRegisterState : ReturnOrderStateBase
{
    public override ReturnOrderStateBase Register(string details)
    {
        return new PendingForExpertRequestConfirmState()
        {
            ReturnOrderStatus = ReturnOrderStatus.Registered,
            Details = details,
            Created = DateTime.Now
        };
    }
}

public class PendingForExpertRequestConfirmState : ReturnOrderStateBase
{
    public override ReturnOrderStateBase Accept(string details)
    {
        return new PendingForSendState()
        {
            ReturnOrderStatus = ReturnOrderStatus.Accepted,
            Details = details,
            Created = DateTime.Now
        };
    }
    public override ReturnOrderStateBase Reject(string details)
    {
        return new PendingForCompletedState()
        {
            ReturnOrderStatus = ReturnOrderStatus.Rejected,
            Details = details,
            Created = DateTime.Now
        };
    }
}

public class PendingForSendState : ReturnOrderStateBase
{
    public override ReturnOrderStateBase Send(string details)
    {
        return new PendingForReceiveState()
        {
            ReturnOrderStatus = ReturnOrderStatus.Sent,
            Details = details,
            Created = DateTime.Now
        };
    }
    public override ReturnOrderStateBase Cancel(string details)
    {
        return new PendingForCompletedState()
        {
            ReturnOrderStatus = ReturnOrderStatus.Canceled,
            Details = details,
            Created = DateTime.Now
        };
    }
}

public class PendingForReceiveState : ReturnOrderStateBase
{
    public override ReturnOrderStateBase Receive(string details)
    {
        return new PendingForExpertProductConfirmState()
        {
            ReturnOrderStatus = ReturnOrderStatus.Received,
            Details = details,
            Created = DateTime.Now
        };
    }
    public override ReturnOrderStateBase Cancel(string details)
    {
        return new PendingForCompletedState()
        {
            ReturnOrderStatus = ReturnOrderStatus.Canceled,
            Details = details,
            Created = DateTime.Now
        };
    }
}
public class PendingForExpertProductConfirmState : ReturnOrderStateBase
{
    public override ReturnOrderStateBase ConfirmAll(string details)
    {
        return new PendingForRefundCostState()
        {
            ReturnOrderStatus = ReturnOrderStatus.AllConfirmed,
            Details = details,
            Created = DateTime.Now
        };
    }
    public override ReturnOrderStateBase ConfirmSome(string details)
    {
        return new PendingForRefundCostState()
        {
            ReturnOrderStatus = ReturnOrderStatus.SomeConfirmed,
            Details = details,
            Created = DateTime.Now
        };
    }
}
public class PendingForRefundCostState : ReturnOrderStateBase
{
    public override ReturnOrderStateBase RefundCost(string details)
    {
        return new PendingForCompletedState()
        {
            ReturnOrderStatus = ReturnOrderStatus.CostRefunded,
            Details = details,
            Created = DateTime.Now
        };
    }
}

public class PendingForCompletedState : ReturnOrderStateBase
{
    public override ReturnOrderStateBase Complete(string details)
    {
        return new CompletedState()
        {
            ReturnOrderStatus = ReturnOrderStatus.Completed,
            Details = details,
            Created = DateTime.Now
        };
    }
}

public class CompletedState : ReturnOrderStateBase
{
}