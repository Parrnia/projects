namespace Onyx.Domain.Entities.OrdersCluster;

public class OrderStateBase : BaseAuditableEntity
{
    public OrderStatus OrderStatus { get; set; }
    public string Details { get; set; } = null!;
    public virtual OrderStateBase Register(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual OrderStateBase UnRegister(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual OrderStateBase ConfirmPayment(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual OrderStateBase UnConfirmPayment(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual OrderStateBase Confirm(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual OrderStateBase Prepare(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual OrderStateBase Ship(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual OrderStateBase Cancel(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual OrderStateBase UnConfirm(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual OrderStateBase RefundCost(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public virtual OrderStateBase Complete(string details) => throw new DomainException("عملیات جاری در این وضعیت امکان پذیر نمی باشد");
    public Order Order { get; set; } = null!;
    public int OrderId { get; set; }
}
public class PendingForRegisterState : OrderStateBase
{
    public override OrderStateBase Register(string details)
    {
        return new PendingForConfirmPaymentState()
        {
            OrderStatus = OrderStatus.OrderRegistered,
            Details = details,
            Created = DateTime.Now
        };
    }
    public override OrderStateBase Cancel(string details)
    {
        return new PendingForCompletedState()
        {
            OrderStatus = OrderStatus.OrderCanceled,
            Details = details,
            Created = DateTime.Now
        };
    }
}

public class PendingForConfirmPaymentState : OrderStateBase
{
    public override OrderStateBase ConfirmPayment(string details)
    {
        return new PendingForConfirmState()
        {
            OrderStatus = OrderStatus.OrderPaymentConfirmed,
            Details = details,
            Created = DateTime.Now
        };
    }
    public override OrderStateBase UnConfirmPayment(string details)
    {
        return new PendingForCompletedState()
        {
            OrderStatus = OrderStatus.OrderPaymentUnconfirmed,
            Details = details,
            Created = DateTime.Now
        };
    }
    public override OrderStateBase UnRegister(string details)
    {
        return new PendingForRegisterState()
        {
            OrderStatus = OrderStatus.PendingForRegister,
            Details = details,
            Created = DateTime.Now
        };
    }
    public override OrderStateBase Cancel(string details)
    {
        return new PendingForRefundState()
        {
            OrderStatus = OrderStatus.OrderCanceled,
            Details = details,
            Created = DateTime.Now
        };
    }

}

public class PendingForConfirmState : OrderStateBase
{
    public override OrderStateBase Confirm(string details)
    {
        return new PendingForPrepareState()
        {
            OrderStatus = OrderStatus.OrderConfirmed,
            Details = details,
            Created = DateTime.Now
        };
    }
    public override OrderStateBase UnConfirm(string details)
    {
        return new PendingForRefundState()
        {
            OrderStatus = OrderStatus.OrderUnconfirmed,
            Details = details,
            Created = DateTime.Now
        };
    }
    public override OrderStateBase Cancel(string details)
    {
        return new PendingForRefundState()
        {
            OrderStatus = OrderStatus.OrderCanceled,
            Details = details,
            Created = DateTime.Now
        };
    }

}

public class PendingForPrepareState : OrderStateBase
{
    public override OrderStateBase Prepare(string details)
    {
        return new PendingForShipState()
        {
            OrderStatus = OrderStatus.OrderPrepared,
            Details = details,
            Created = DateTime.Now
        };
    }
    public override OrderStateBase Cancel(string details)
    {
        return new PendingForRefundState()
        {
            OrderStatus = OrderStatus.OrderCanceled,
            Details = details,
            Created = DateTime.Now
        };
    }
}
public class PendingForShipState : OrderStateBase
{
    public override OrderStateBase Ship(string details)
    {
        return new PendingForCompletedState()
        {
            OrderStatus = OrderStatus.OrderShipped,
            Details = details,
            Created = DateTime.Now
        };
    }
    public override OrderStateBase Cancel(string details)
    {
        return new PendingForRefundState()
        {
            OrderStatus = OrderStatus.OrderCanceled,
            Details = details,
            Created = DateTime.Now
        };
    }
}
public class PendingForRefundState : OrderStateBase
{
    public override OrderStateBase RefundCost(string details)
    {
        return new PendingForCompletedState()
        {
            OrderStatus = OrderStatus.OrderCostRefunded,
            Details = details,
            Created = DateTime.Now
        };
    }
}

public class PendingForCompletedState : OrderStateBase
{
    public override OrderStateBase Complete(string details)
    {
        return new CompletedState()
        {
            OrderStatus = OrderStatus.OrderCompleted,
            Details = details,
            Created = DateTime.Now
        };
    }
}

public class CompletedState : OrderStateBase
{
}