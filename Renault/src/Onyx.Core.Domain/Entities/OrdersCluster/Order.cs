using MediatR;
using Onyx.Domain.Entities.OrdersCluster.Payments;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Domain.Entities.OrdersCluster;

public class Order : BaseAuditableEntity
{
    public Order()
    {
        CreatedAt = DateTime.Now;
        IsPayed = false;
        OrderStateHistory = new List<OrderStateBase>();
    }

    /// <summary>
    /// رمز مبادله با سون
    /// </summary>
    public string Token { get; set; } = null!;

    /// <summary>
    /// شماره
    /// </summary>
    public string Number { get; set; } = null!;

    /// <summary>
    /// تعداد
    /// </summary>
    public double Quantity { get; set; }

    /// <summary>
    /// جمع قیمت کل محصولات
    /// </summary>
    public decimal Subtotal { get; set; }

    /// <summary>
    /// درصد تخفیف محاسبه شده بر اساس نقش
    /// </summary>
    public double DiscountPercentForRole { get; set; }

    /// <summary>
    /// مبلغ پرداختی
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// زمان سفارش
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// پرداخت ها
    /// </summary>
    public List<OrderPayment> PaymentMethods { get; set; } = new();

    /// <summary>
    /// شیوه پرداخت
    /// </summary>
    public OrderPaymentType OrderPaymentType { get; set; }

    /// <summary>
    /// پرداخت شده
    /// </summary>
    public bool IsPayed { get; set; }

    /// <summary>
    /// تاریخچه وضعیت سفارش
    /// </summary>
    public List<OrderStateBase> OrderStateHistory { get; private set; }

    /// <summary>
    /// آدرس تحویل کالا
    /// </summary>
    public OrderAddress OrderAddress { get; set; } = null!;

    /// <summary>
    /// آیتم های سفارش
    /// </summary>
    public List<OrderItem> Items { get; set; } = new();

    /// <summary>
    /// هزینه های جانبی سفارش
    /// </summary>
    public List<OrderTotal> Totals { get; set; } = new();

    /// <summary>
    /// مشتری سفارش دهنده
    /// </summary>
    public Customer Customer { get; set; } = null!;

    public Guid CustomerId { get; set; }

    /// <summary>
    /// نوع مشتری در زمان سفارش
    /// </summary>
    public CustomerTypeEnum CustomerTypeEnum { get; set; }

    /// <summary>
    /// شماره برای ارسال پیامک
    /// </summary>
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// نام مشتری
    /// </summary>
    public string CustomerFirstName { get; set; } = null!;

    /// <summary>
    /// نام خانوادگی مشتری یا کد اقتصادی
    /// </summary>
    public string CustomerLastName { get; set; } = null!;

    /// <summary>
    /// کد ملی
    /// </summary>
    public string NationalCode { get; set; } = null!;

    /// <summary>
    /// نوع هویت مشتری
    /// </summary>
    public PersonType PersonType { get; set; }

    /// <summary>
    /// درصد مالیات
    /// </summary>
    public double TaxPercent { get; set; }

    /// <summary>
    /// درخواست های بازگشت کالا
    /// </summary>
    public List<ReturnOrder> ReturnOrders { get; set; } = new();

    public OrderStateBase GetCurrentOrderState()
    {
        return OrderStateHistory.OrderBy(e => e.Created).Last();
    }

    public void AddOrderPayment(OrderPayment payment, OrderPaymentType orderPaymentType)
    {
        PaymentMethods.Add(payment);
        OrderPaymentType = orderPaymentType;
    }
    public void SetQuantity()
    {
        Quantity = Items.Sum(e => e.Quantity);
    }
    public void SetSubtotal()
    {
        Subtotal = Items.Sum(c => c.Total);
    }
    public void SetTotal()
    {
        Total = Subtotal;
        ApplyTotals();
    }
    public void ApplyTotals()
    {
        var orderTotalTax = Totals.SingleOrDefault(c => c.Type == OrderTotalType.Tax);
        if (orderTotalTax != null)
        {
            Total += orderTotalTax.Price;
        }
        var orderTotalDiscountOnProduct = Totals.SingleOrDefault(c => c.Type == OrderTotalType.DiscountOnProduct);
        if (orderTotalDiscountOnProduct != null)
        {
            Total -= orderTotalDiscountOnProduct.Price;
        }
        var orderTotalDiscountOnCustomerType = Totals.SingleOrDefault(c => c.Type == OrderTotalType.DiscountOnCustomerType);
        if (orderTotalDiscountOnCustomerType != null)
        {
            Total -= orderTotalDiscountOnCustomerType.Price;
        }
        var deliveryOrderTotal = Totals.SingleOrDefault(c => c.Type == OrderTotalType.Shipping);
        if (deliveryOrderTotal != null)
        {
            Total += deliveryOrderTotal.Price;
        }
    }
    public void SetTaxTotal()
    {
        var orderTotalTaxDb = Totals.SingleOrDefault(c => c.Type == OrderTotalType.Tax);

        var orderTotalTax = orderTotalTaxDb ??
                            new OrderTotal()
                            {
                                Title = "مالیات",
                                Type = OrderTotalType.Tax
                            };
        if (Items.Any())
        {
            foreach (var item in Items)
            {
                var totalDiscountPercent = item.DiscountPercentForProduct + DiscountPercentForRole;
                orderTotalTax.Price += CalculateTax(item.Total, totalDiscountPercent);
            }
        }
        else
        {
            orderTotalTax.Price = 0;
        }

        if (orderTotalTaxDb == null)
        {
            Totals.Add(orderTotalTax);
        }
    }

    public void SetDiscountOnProductTotal()
    {
        var orderTotalDiscountOnProductDb = Totals.SingleOrDefault(c => c.Type == OrderTotalType.DiscountOnProduct);

        var orderTotalDiscountOnProduct = orderTotalDiscountOnProductDb ??
                                          new OrderTotal()
                                          {
                                              Title = "تخفیف بر روی محصول",
                                              Type = OrderTotalType.DiscountOnProduct
                                          };
        if (Items.Any())
        {
            foreach (var item in Items)
            {
                orderTotalDiscountOnProduct.Price += CalculateDiscount(item.Total, item.DiscountPercentForProduct);
            }
        }
        else
        {
            orderTotalDiscountOnProduct.Price = 0;
        }
        
        if (orderTotalDiscountOnProductDb == null)
        {
            Totals.Add(orderTotalDiscountOnProduct);
        }
    }

    public void SetDiscountOnCustomerTypeTotal()
    {
        var orderTotalDiscountOnCustomerTypeDb = Totals.SingleOrDefault(c => c.Type == OrderTotalType.DiscountOnCustomerType);

        var orderTotalDiscountOnCustomerType = orderTotalDiscountOnCustomerTypeDb ??
                                          new OrderTotal()
                                          {
                                              Title = "تخفیف نوع مشتری",
                                              Type = OrderTotalType.DiscountOnCustomerType
                                          };
        SetSubtotal();

        orderTotalDiscountOnCustomerType.Price = Items.Any() ? CalculateDiscount(Subtotal, DiscountPercentForRole) : 0;

        if (orderTotalDiscountOnCustomerTypeDb == null)
        {
            Totals.Add(orderTotalDiscountOnCustomerType);
        }
    }

    public void SetDeliveryTotal()
    {
        var orderTotalDeliveryDb = Totals.SingleOrDefault(c => c.Type == OrderTotalType.Shipping);

        var orderTotalDelivery = orderTotalDeliveryDb ??
                                 new OrderTotal
                                 {
                                     Title = "هزینه ارسال",
                                     Type = OrderTotalType.Shipping
                                 };
        SetDiscountOnCustomerTypeTotal();
        SetDiscountOnProductTotal();
        orderTotalDelivery.Price = Subtotal > 0 ? CalculateDeliveryCost(Subtotal, Totals) : 0;
        if (orderTotalDeliveryDb == null)
        {
            Totals.Add(orderTotalDelivery);
        }
    }

    public decimal CalculateTax(decimal cost, double discountPercent)
    {
        var tax = cost * ((decimal)TaxPercent / 100) * (1 - (decimal)discountPercent / 100);
        return tax;
    }

    public decimal CalculateDiscount(decimal cost, double discountPercent)
    {
        return cost * (decimal)discountPercent / 100;
    }

    public decimal CalculateDeliveryCost(decimal orderCost, List<OrderTotal> totals)
    {
        var orderTotalDiscountOnProduct = totals.SingleOrDefault(c => c.Type == OrderTotalType.DiscountOnProduct);
        if (orderTotalDiscountOnProduct != null)
        {
            orderCost -= orderTotalDiscountOnProduct.Price;
        }
        var orderTotalDiscountOnCustomerType = totals.SingleOrDefault(c => c.Type == OrderTotalType.DiscountOnCustomerType);
        if (orderTotalDiscountOnCustomerType != null)
        {
            orderCost -= orderTotalDiscountOnCustomerType.Price;
        }
        switch (orderCost)
        {
            case >= 100000000:
                return 0;
            case >= 50000000:
                return 600000;
            default:
                return 1000000;
        }
    }

    public bool RegisterOrder(string details, CustomerTypeEnum customerTypeEnum)
    {
        var state = GetOrderState(GetCurrentOrderState());
        if (!SellItems(customerTypeEnum))
        {
            return false;
        }

        OrderStateHistory.Add(state.Register(details));
        return true;
    }
    public void UnRegisterOrder(string details)
    {
        var state = GetOrderState(GetCurrentOrderState());
        ReturnItems();
        OrderStateHistory.Add(state.UnRegister(details));
    }
    public void ConfirmOrderPayment(string details)
    {
        var state = GetOrderState(GetCurrentOrderState());

        OrderStateHistory.Add(state.ConfirmPayment(details));
    }
    public void ConfirmOrder(string details)
    {
        var state = GetOrderState(GetCurrentOrderState());

        OrderStateHistory.Add(state.Confirm(details));
    }
    public void PrepareOrder(string details)
    {
        var state = GetOrderState(GetCurrentOrderState());

        OrderStateHistory.Add(state.Prepare(details));
    }
    public void ShipOrder(string details)
    {
        var state = GetOrderState(GetCurrentOrderState());

        OrderStateHistory.Add(state.Ship(details));
    }
    public void Cancel(string details)
    {
        var state = GetOrderState(GetCurrentOrderState());
        switch (state.OrderStatus)
        {
            case OrderStatus.OrderShipped:
                break;
            case OrderStatus.OrderCostRefunded:
                break;
            case OrderStatus.OrderUnconfirmed:
                break;
            case OrderStatus.OrderCompleted:
                break;
            default:
                ReturnItems();
                OrderStateHistory.Add(state.Cancel(details));
                break;
        }
    }
    public void UnConfirmOrderPayment(string details)
    {
        var state = GetOrderState(GetCurrentOrderState());
        switch (state.OrderStatus)
        {
            case OrderStatus.OrderRegistered:
                ReturnItems();
                OrderStateHistory.Add(state.UnConfirmPayment(details));
                break;
        }
    }
    public void UnConfirmOrder(string details)
    {
        var state = GetOrderState(GetCurrentOrderState());
        switch (state.OrderStatus)
        {
            case OrderStatus.OrderPaymentConfirmed:
                ReturnItems();
                OrderStateHistory.Add(state.UnConfirm(details));
                break;
        }
    }
    public void RefundCost(string details)
    {
        if (PaymentMethods.Any(c => c.PaymentType == PaymentType.Credit))
        {
            var successFul = Customer.AddCredit(Total, CustomerId, CustomerFirstName + " " + CustomerLastName, Number);
            if (!successFul)
            {
                throw new DomainException("مقدار اعتبار نمی تواند کمتر از صفر باشد");
            }
        }
        else
        {
            throw new DomainException("مبلغ سفارش نمی تواند به اعتبار مشتری اضافه شود");
        }

        var state = GetOrderState(GetCurrentOrderState());
        switch (state.OrderStatus)
        {
            case OrderStatus.OrderCanceled:
                OrderStateHistory.Add(state.RefundCost(details));
                break;
            case OrderStatus.OrderUnconfirmed:
                OrderStateHistory.Add(state.RefundCost(details));
                break;
        }
    }

    public void Complete(string details)
    {
        var state = GetOrderState(GetCurrentOrderState());
        switch (state.OrderStatus)
        {
            case OrderStatus.PendingForRegister:
                break;
            case OrderStatus.OrderRegistered:
                break;
            case OrderStatus.OrderPaymentConfirmed:
                break;
            case OrderStatus.OrderConfirmed:
                break;
            case OrderStatus.OrderPrepared:
                break;
            case OrderStatus.OrderUnconfirmed:
                break;
            case OrderStatus.OrderCanceled:
                break;
            default:
                OrderStateHistory.Add(state.Complete(details));
                break;
        }
    }

    private void ReturnItems()
    {
        foreach (var orderItem in Items)
        {
            orderItem.ProductAttributeOption.Return(orderItem.Quantity);
        }
    }
    private bool SellItems(CustomerTypeEnum customerTypeEnum)
    {
        foreach (var orderItem in Items)
        {
            var successfulSale = orderItem.ProductAttributeOption.Sell(orderItem.Quantity, customerTypeEnum);
            if (!successfulSale)
            {
                return false;
            }
        }
        return true;
    }
    private static OrderStateBase GetOrderState(OrderStateBase orderStateBase)
    {
        switch (orderStateBase.OrderStatus)
        {
            case OrderStatus.PendingForRegister:
                return new PendingForRegisterState() { OrderStatus = orderStateBase.OrderStatus, Details = orderStateBase.Details };
            case OrderStatus.OrderRegistered:
                return new PendingForConfirmPaymentState() { OrderStatus = orderStateBase.OrderStatus, Details = orderStateBase.Details };
            case OrderStatus.OrderPaymentConfirmed:
                return new PendingForConfirmState() { OrderStatus = orderStateBase.OrderStatus, Details = orderStateBase.Details };
            case OrderStatus.OrderConfirmed:
                return new PendingForPrepareState() { OrderStatus = orderStateBase.OrderStatus, Details = orderStateBase.Details };
            case OrderStatus.OrderPrepared:
                return new PendingForShipState() { OrderStatus = orderStateBase.OrderStatus, Details = orderStateBase.Details };
            case OrderStatus.OrderPaymentUnconfirmed:
                return new PendingForCompletedState() { OrderStatus = orderStateBase.OrderStatus, Details = orderStateBase.Details };
            case OrderStatus.OrderUnconfirmed:
                return new PendingForRefundState() { OrderStatus = orderStateBase.OrderStatus, Details = orderStateBase.Details };
            case OrderStatus.OrderCanceled:
                return new PendingForRefundState() { OrderStatus = orderStateBase.OrderStatus, Details = orderStateBase.Details };
            case OrderStatus.OrderShipped:
                return new PendingForCompletedState() { OrderStatus = orderStateBase.OrderStatus, Details = orderStateBase.Details };
            case OrderStatus.OrderCostRefunded:
                return new PendingForCompletedState() { OrderStatus = orderStateBase.OrderStatus, Details = orderStateBase.Details };
            case OrderStatus.OrderCompleted:
                return new CompletedState() { OrderStatus = orderStateBase.OrderStatus, Details = orderStateBase.Details };
            default:
                return new CompletedState();
        }
    }
}