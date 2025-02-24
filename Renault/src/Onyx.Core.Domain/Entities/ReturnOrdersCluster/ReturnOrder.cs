using System.Text.RegularExpressions;
using MediatR;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.OrdersCluster.Payments;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Domain.Entities.ReturnOrdersCluster;

public class ReturnOrder : BaseAuditableEntity
{
    public ReturnOrder()
    {
        CreatedAt = DateTime.Now;
    }
    /// <summary>
    /// رمز مبادله با سون
    /// </summary>
    public string? Token { get; set; }
    /// <summary>
    /// شماره
    /// </summary>
    public string Number { get; set; } = null!;
    /// <summary>
    /// تعداد
    /// </summary>
    public double Quantity { get; private set; }
    /// <summary>
    /// جمع قیمت کل محصولات
    /// </summary>
    public decimal Subtotal { get; private set; }
    /// <summary>
    /// مبلغ پرداختی
    /// </summary>
    public decimal Total { get; private set; }
    /// <summary>
    /// زمان ثبت بازگشت سفارش
    /// </summary>
    public DateTime CreatedAt { get; }
    /// <summary>
    /// شیوه بازپرداخت
    /// </summary>
    public CostRefundType CostRefundType { get; set; }
    /// <summary>
    /// تاریخچه وضعیت بازگشت سفارش
    /// </summary>
    public List<ReturnOrderStateBase> ReturnOrderStateHistory { get; private set; } = new List<ReturnOrderStateBase>();
    /// <summary>
    /// شیوه بازگشت                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       شیوه بازگشت کالا
    /// </summary>
    public ReturnOrderTransportationType ReturnOrderTransportationType { get; set; }
    /// <summary>
    /// آیتم های بازگشت سفارش
    /// </summary>
    public List<ReturnOrderItemGroup> ItemGroups { get; set; } = new List<ReturnOrderItemGroup>();
    /// <summary>
    /// هزینه های جانبی بازگشت سفارش
    /// </summary>
    public List<ReturnOrderTotal> Totals { get; set; } = new List<ReturnOrderTotal>();
    /// <summary>
    /// اطلاعات حساب
    /// </summary>
    public string? CustomerAccountInfo { get; set; }
    /// <summary>
    /// سفارش مربوطه
    /// </summary>
    public Order Order { get; set; } = null!;
    public int OrderId { get; set; }

    public void SetQuantity()
    {
        Quantity = ItemGroups.SelectMany(c => c.ReturnOrderItems.Where(e => e.IsAccepted))
            .Sum(e => e.Quantity);
    }

    public void SetSubtotal()
    {
        Subtotal = ItemGroups.SelectMany(c => c.ReturnOrderItems.Where(e => e.IsAccepted))
            .Sum(e => e.Total);
    }
    public void SetTotal()
    {
        Total = Subtotal;
        ApplyTotals();
    }

    private void ApplyTotals()
    {
        foreach (var returnOrderTotal in Totals)
        {
            switch (returnOrderTotal.ReturnOrderTotalApplyType)
            {
                case ReturnOrderTotalApplyType.AddToTotal:
                    Total += returnOrderTotal.Price;
                    break;
                case ReturnOrderTotalApplyType.ReduceFromTotal:
                    Total -= returnOrderTotal.Price;
                    break;
                case ReturnOrderTotalApplyType.DoNothing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void AddReturnOrderItemWithoutValidation(ReturnOrderItem returnOrderItem, int returnOrderItemGroupId)
    {
        var itemGroup = ItemGroups.SingleOrDefault(c => c.Id == returnOrderItemGroupId);
        if (itemGroup != null)
        {
            returnOrderItem.SetTotal(itemGroup.Price);
            itemGroup.ReturnOrderItems.Add(returnOrderItem);
            SetQuantity();
            SetSubtotal();
            SetTotal();
        }
    }
    public void RemoveReturnOrderItemWithoutValidation(ReturnOrderItem returnOrderItem)
    {
        var itemGroup = ItemGroups.SingleOrDefault(c => c.Id == returnOrderItem.ReturnOrderItemGroupId);
        var item = itemGroup?.ReturnOrderItems.SingleOrDefault(c => c.Id == returnOrderItem.Id);
        if (item != null)
        {
            itemGroup?.ReturnOrderItems.Remove(item);
            SetQuantity();
            SetSubtotal();
            SetTotal();
        }
    }
    public void UpdateReturnOrderItemWithoutValidation(int id, int returnOrderItemGroupId, double quantity, string details, bool isAccepted, ReturnOrderCustomerReasonType? returnOrderCustomerReasonType, ReturnOrderOrganizationReasonType? returnOrderOrganizationReasonType)
    {
        var itemGroup = ItemGroups.SingleOrDefault(c => c.Id == returnOrderItemGroupId);
        var item = itemGroup?.ReturnOrderItems.SingleOrDefault(c => c.Id == id);
        if (item != null)
        {
            item.Quantity = quantity;
            item.IsAccepted = isAccepted;
            item.ReturnOrderReason.Details = details;
            item.ReturnOrderReason.SetReturnOrderReasonType(returnOrderCustomerReasonType, returnOrderOrganizationReasonType);

            if (itemGroup != null)
            {
                item.SetTotal(itemGroup.Price);
            }

            SetQuantity();
            SetSubtotal();
            SetTotal();
        }
    }

    public void AddReturnOrderItemDocumentWithoutValidation(ReturnOrderItemDocument returnOrderItemDocument, int returnOrderItemGroupId, int returnOrderItemId)
    {
        var returnOrderItemGroup = ItemGroups.SingleOrDefault(c => c.Id == returnOrderItemGroupId);

        var returnOrderItem = returnOrderItemGroup?.ReturnOrderItems.SingleOrDefault(c => c.Id == returnOrderItemId);

        returnOrderItem?.ReturnOrderItemDocuments.Add(returnOrderItemDocument);

    }
    public void RemoveReturnOrderItemDocumentWithoutValidation(ReturnOrderItemDocument returnOrderItemDocument, int returnOrderItemGroupId, int returnOrderItemId)
    {
        var returnOrderItemGroup = ItemGroups.SingleOrDefault(c => c.Id == returnOrderItemGroupId);

        var returnOrderItem = returnOrderItemGroup?.ReturnOrderItems.SingleOrDefault(c => c.Id == returnOrderItemId);

        var document = returnOrderItem?.ReturnOrderItemDocuments.SingleOrDefault(c => c.Id == returnOrderItemDocument.Id);
        if (document != null)
        {
            returnOrderItem?.ReturnOrderItemDocuments.Remove(document);
        }
    }
    public void UpdateReturnOrderItemDocumentWithoutValidation(int id, Guid image, string description, int returnOrderItemGroupId, int returnOrderItemId)
    {
        var returnOrderItemGroup = ItemGroups.SingleOrDefault(c => c.Id == returnOrderItemGroupId);

        var returnOrderItem = returnOrderItemGroup?.ReturnOrderItems.SingleOrDefault(c => c.Id == returnOrderItemId);

        var returnOrderItemDocument = returnOrderItem?.ReturnOrderItemDocuments.SingleOrDefault(c => c.Id == id);

        if (returnOrderItemDocument != null)
        {
            returnOrderItemDocument.Image = image;
            returnOrderItemDocument.Description = description;
        }
    }

    public void AddTotalWithoutValidation(ReturnOrderTotal returnOrderTotal)
    {
        Totals.Add(returnOrderTotal);
        SetTotal();
    }
    public void RemoveTotalWithoutValidation(ReturnOrderTotal returnOrderTotal)
    {
        var total = Totals.SingleOrDefault(c => c.Id == returnOrderTotal.Id);
        if (total != null)
        {
            Totals.Remove(total);
            SetTotal();
        }
    }
    public void UpdateTotalWithoutValidation(int id, string title, decimal price, ReturnOrderTotalType type, ReturnOrderTotalApplyType returnOrderTotalApplyType)
    {
        var total = Totals.SingleOrDefault(c => c.Id == id);
        if (total != null)
        {
            total.Price = price;
            total.Title = title;
            total.Type = type;
            total.ReturnOrderTotalApplyType = returnOrderTotalApplyType;
            SetTotal();
        }
    }

    public void AddReturnOrderTotalDocumentWithoutValidation(ReturnOrderTotalDocument returnOrderTotalDocument, int returnOrderTotalId)
    {
        var returnOrderTotal = Totals.SingleOrDefault(c => c.Id == returnOrderTotalId);

        returnOrderTotal?.ReturnOrderTotalDocuments.Add(returnOrderTotalDocument);

    }
    public void RemoveReturnOrderTotalDocumentWithoutValidation(ReturnOrderTotalDocument returnOrderTotalDocument, int returnOrderTotalId)
    {

        var returnOrderTotal = Totals.SingleOrDefault(c => c.Id == returnOrderTotalId);

        var document = returnOrderTotal?.ReturnOrderTotalDocuments.SingleOrDefault(c => c.Id == returnOrderTotalDocument.Id);
        if (document != null)
        {
            returnOrderTotal?.ReturnOrderTotalDocuments.Remove(document);
        }
    }
    public void UpdateReturnOrderTotalDocumentWithoutValidation(int id, Guid image, string description, int returnOrderTotalId)
    {
        var returnOrderTotal = Totals.SingleOrDefault(c => c.Id == returnOrderTotalId);

        var returnOrderTotalDocument = returnOrderTotal?.ReturnOrderTotalDocuments.SingleOrDefault(c => c.Id == id);

        if (returnOrderTotalDocument != null)
        {
            returnOrderTotalDocument.Image = image;
            returnOrderTotalDocument.Description = description;
        }
    }

    public void SetTaxTotal()
    {
        var returnOrderTotalTaxDb = Totals.SingleOrDefault(c => c.Type == ReturnOrderTotalType.Tax);

        var returnOrderTotalTax = returnOrderTotalTaxDb ??
            new ReturnOrderTotal()
            {
                Title = "مالیات",
                Type = ReturnOrderTotalType.Tax,
                ReturnOrderTotalApplyType = ReturnOrderTotalApplyType.AddToTotal
            };
        foreach (var returnOrderItemGroup in ItemGroups)
        {
            var orderItemDb = Order.Items.SingleOrDefault(c => c.ProductAttributeOptionId == returnOrderItemGroup.ProductAttributeOptionId);
            if (orderItemDb != null)
            {
                var totalDiscountPercent = orderItemDb.DiscountPercentForProduct + Order.DiscountPercentForRole;
                foreach (var returnOrderItem in returnOrderItemGroup.ReturnOrderItems.Where(c => c.IsAccepted))
                {
                    returnOrderTotalTax.Price += CalculateTax(returnOrderItem.Total, totalDiscountPercent);
                }
            }
        }
        if (returnOrderTotalTaxDb == null)
        {
            Totals.Add(returnOrderTotalTax);
        }
    }

    public void SetDiscountTotal()
    {
        var returnOrderTotalDiscountDb = Totals.SingleOrDefault(c => c.Type == ReturnOrderTotalType.TotalDiscount);
        var returnOrderTotalDiscount = returnOrderTotalDiscountDb ??
            new ReturnOrderTotal()
            {
                Title = "تخفیف کل",
                Type = ReturnOrderTotalType.TotalDiscount,
                ReturnOrderTotalApplyType = ReturnOrderTotalApplyType.ReduceFromTotal
            };
        foreach (var returnOrderItemGroup in ItemGroups)
        {
            var orderItemDb = Order.Items.SingleOrDefault(c => c.ProductAttributeOptionId == returnOrderItemGroup.ProductAttributeOptionId);
            if (orderItemDb != null)
            {
                var totalDiscountPercent = orderItemDb.DiscountPercentForProduct + Order.DiscountPercentForRole;
                foreach (var returnOrderItem in returnOrderItemGroup.ReturnOrderItems.Where(c => c.IsAccepted))
                {
                    returnOrderTotalDiscount.Price += CalculateDiscount(returnOrderItem.Total, totalDiscountPercent);
                }
            }
        }

        if (returnOrderTotalDiscountDb == null)
        {
            Totals.Add(returnOrderTotalDiscount);
        }
    }

    public void SetDeliveryTotal()
    {
        var returnOrderTotalDeliveryDb = Totals.SingleOrDefault(c => c.Type == ReturnOrderTotalType.Shipping);
        if (returnOrderTotalDeliveryDb == null)
        {
            var orderTotalDelivery = Order.Totals.SingleOrDefault(c => c.Type == OrderTotalType.Shipping);
            if (orderTotalDelivery != null)
            {
                var returnOrderTotalDelivery = new ReturnOrderTotal
                {
                    Title = "ارسال",
                    Price = orderTotalDelivery.Price,
                    Type = ReturnOrderTotalType.Shipping,
                    ReturnOrderTotalApplyType = ReturnOrderTotalApplyType.DoNothing
                };
                Totals.Add(returnOrderTotalDelivery);
            }
        }
    }

    public decimal CalculateTax(decimal cost, double discountPercent)
    {
        var tax = cost * ((decimal)Order.TaxPercent / 100) * (1 - (decimal)discountPercent / 100);
        return tax;
    }

    public decimal CalculateDiscount(decimal cost, double discountPercent)
    {
        return cost * (decimal)discountPercent / 100;
    }

    public bool CheckCount()
    {
        foreach (var itemGroup in ItemGroups)
        {
            var orderItemDb = Order.Items.SingleOrDefault(c => c.ProductAttributeOptionId == itemGroup.ProductAttributeOptionId);
            if (orderItemDb != null)
            {
                var count = itemGroup.ReturnOrderItems.Sum(orderItem => orderItem.Quantity);
                if (count > orderItemDb.Quantity)
                {
                    return false;
                }
            }

            var totalQuantityDb = itemGroup.TotalQuantity;
            var newTotalQuantity = itemGroup.ReturnOrderItems.Sum(c => c.Quantity);
            if (Math.Abs(newTotalQuantity - totalQuantityDb) > 0)
            {
                return false;
            }
            itemGroup.SetTotalQuantity();
        }

        
        return true;
    }

    public bool CheckReturnOrderTotals()
    {
        foreach (var returnOrderTotal in Totals.Where(c => c.Type != ReturnOrderTotalType.Tax && c.Type != ReturnOrderTotalType.TotalDiscount))
        {
            if (!returnOrderTotal.ReturnOrderTotalDocuments.Any())
            {
                return false;
            }
        }
        return true;
    }

    public ReturnOrderStateBase GetCurrentReturnOrderState()
    {
        return ReturnOrderStateHistory.OrderBy(e => e.Created).Last();
    }

    public bool Register(string details)
    {
        var state = GetReturnOrderState(GetCurrentReturnOrderState());

        ReturnOrderStateHistory.Add(state.Register(details));
        return true;
    }
    public void Accept(string details)
    {
        var state = GetReturnOrderState(GetCurrentReturnOrderState());

        ReturnOrderStateHistory.Add(state.Accept(details));
    }
    public void Reject(string details)
    {
        var state = GetReturnOrderState(GetCurrentReturnOrderState());

        ReturnOrderStateHistory.Add(state.Reject(details));
    }
    public void Send(string details)
    {
        var state = GetReturnOrderState(GetCurrentReturnOrderState());

        ReturnOrderStateHistory.Add(state.Send(details));
    }
    public void Receive(string details)
    {
        var state = GetReturnOrderState(GetCurrentReturnOrderState());

        ReturnOrderStateHistory.Add(state.Receive(details));
    }
    public void ConfirmAll(string details)
    {
        var state = GetReturnOrderState(GetCurrentReturnOrderState());
        ReturnItems();
        ReturnOrderStateHistory.Add(state.ConfirmAll(details));
    }
    public void ConfirmSome(string details)
    {
        var state = GetReturnOrderState(GetCurrentReturnOrderState());
        ReturnItems();
        ReturnOrderStateHistory.Add(state.ConfirmSome(details));
    }
    public void RefundCost(string details)
    {
        if (Order.PaymentMethods is CreditPayment)
        {
            var successFul = Order.Customer.AddCredit(Total, Order.CustomerId, Order.CustomerFirstName + " " + Order.CustomerLastName, Number);
            if (!successFul)
            {
                throw new DomainException("مقدار اعتبار نمی تواند کمتر از صفر باشد");
            }
        }
        var state = GetReturnOrderState(GetCurrentReturnOrderState());
        ReturnOrderStateHistory.Add(state.RefundCost(details));
    }
    public void Cancel(string details)
    {
        var state = GetReturnOrderState(GetCurrentReturnOrderState());
        ReturnOrderStateHistory.Add(state.Cancel(details));
    }
    public void Complete(string details)
    {
        var state = GetReturnOrderState(GetCurrentReturnOrderState());
        ReturnOrderStateHistory.Add(state.Complete(details));
    }


    private void ReturnItems()
    {
        foreach (var itemGroup in ItemGroups)
        {
            foreach (var orderItem in itemGroup.ReturnOrderItems.Where(c => c.IsAccepted))
            {
                itemGroup.ProductAttributeOption.Return(orderItem.Quantity);
            }
        }
    }

    private static ReturnOrderStateBase GetReturnOrderState(ReturnOrderStateBase orderStateBase)
    {
        switch (orderStateBase.ReturnOrderStatus)
        {
            case ReturnOrderStatus.PendingForRegister:
                return new PendingForRegisterState() { ReturnOrderStatus = orderStateBase.ReturnOrderStatus, Details = orderStateBase.Details };
            case ReturnOrderStatus.Registered:
                return new PendingForExpertRequestConfirmState() { ReturnOrderStatus = orderStateBase.ReturnOrderStatus, Details = orderStateBase.Details };
            case ReturnOrderStatus.Rejected:
                return new PendingForCompletedState() { ReturnOrderStatus = orderStateBase.ReturnOrderStatus, Details = orderStateBase.Details };
            case ReturnOrderStatus.Accepted:
                return new PendingForSendState() { ReturnOrderStatus = orderStateBase.ReturnOrderStatus, Details = orderStateBase.Details };
            case ReturnOrderStatus.Sent:
                return new PendingForReceiveState() { ReturnOrderStatus = orderStateBase.ReturnOrderStatus, Details = orderStateBase.Details };
            case ReturnOrderStatus.Received:
                return new PendingForExpertProductConfirmState() { ReturnOrderStatus = orderStateBase.ReturnOrderStatus, Details = orderStateBase.Details };
            case ReturnOrderStatus.AllConfirmed:
                return new PendingForRefundCostState() { ReturnOrderStatus = orderStateBase.ReturnOrderStatus, Details = orderStateBase.Details };
            case ReturnOrderStatus.CostRefunded:
                return new PendingForCompletedState() { ReturnOrderStatus = orderStateBase.ReturnOrderStatus, Details = orderStateBase.Details };
            case ReturnOrderStatus.SomeConfirmed:
                return new PendingForRefundCostState() { ReturnOrderStatus = orderStateBase.ReturnOrderStatus, Details = orderStateBase.Details };
            case ReturnOrderStatus.Canceled:
                return new PendingForCompletedState() { ReturnOrderStatus = orderStateBase.ReturnOrderStatus, Details = orderStateBase.Details };
            case ReturnOrderStatus.Completed:
                return new CompletedState() { ReturnOrderStatus = orderStateBase.ReturnOrderStatus, Details = orderStateBase.Details };
            default:
                return new CompletedState();
        }
    }
}