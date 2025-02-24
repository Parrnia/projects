using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Services;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.OrdersCluster.Payments;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;
using PendingForRegisterState = Onyx.Domain.Entities.ReturnOrdersCluster.PendingForRegisterState;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.CreateReturnOrder;
public record CreateReturnOrderCommand : IRequest<int>
{
    public List<CreateItemGroupCommandForReturnOrder> ItemGroups { get; init; } = new List<CreateItemGroupCommandForReturnOrder>();
    public int OrderId { get; init; }
    public Guid CustomerId { get; init; }
    public string Details { get; init; } = null!;
    public double Quantity { get; init; }
    public decimal Subtotal { get; init; }
    public decimal Total { get; init; }
    public string? CustomerAccountInfo { get; init; }
}

public class CreateReturnOrderCommandHandler : IRequestHandler<CreateReturnOrderCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICreateReturnOrderHelper _createReturnOrderHelper;
    private readonly ISmsService _smsService;
    private readonly IFileStore _fileStore;

    public CreateReturnOrderCommandHandler(IApplicationDbContext context, ICreateReturnOrderHelper createReturnOrderHelper, ISmsService smsService, IFileStore fileStore)
    {
        _context = context;
        _createReturnOrderHelper = createReturnOrderHelper;
        _smsService = smsService;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateReturnOrderCommand request, CancellationToken cancellationToken)
    {
        //todo:  (e.OrderStateHistory.OrderBy(d => d.Created).Last().OrderStatus == OrderStatus.OrderCompleted || e.OrderStateHistory.OrderBy(d => d.Created).Last().OrderStatus == OrderStatus.OrderShipped), cancellationToken);
        var orderDb = await _context.Orders
            .Include(c => c.OrderStateHistory)
            .Include(e => e.Items).ThenInclude(c => c.ProductAttributeOption)
            .Include(c => c.Totals)
            .SingleOrDefaultAsync(e => e.Id == request.OrderId &&
                                       (e.OrderStateHistory.OrderBy(d => d.Created).Last().OrderStatus == OrderStatus.OrderCompleted || e.OrderStateHistory.OrderBy(d => d.Created).Last().OrderStatus == OrderStatus.OrderShipped || true), cancellationToken);
        if (orderDb == null)
        {
            throw new BadCommandException("سفارش مورد نظر یافت نشد یا قابل مرجوعی نیست");
        }

        if (request.CustomerId != orderDb.CustomerId)
        {
            throw new ForbiddenAccessException("سفارش");
        }

        var returnOrderDbs = await _context.ReturnOrders
            .Include(c => c.ItemGroups)
            .ThenInclude(c => c.ReturnOrderItems)
            .Where(e => e.OrderId == request.OrderId)
            .Include(c => c.ReturnOrderStateHistory)
            .ToListAsync(cancellationToken);

        var returnOrderItemDbs = returnOrderDbs
            .SelectMany(c => c.ItemGroups)
            .SelectMany(e => e.ReturnOrderItems).ToList();

        foreach (var itemGroup in request.ItemGroups)
        {
            var orderItemDb = orderDb.Items
                .SingleOrDefault(c => c.ProductAttributeOptionId == itemGroup.ProductAttributeOptionId);
            if (orderItemDb == null)
            {
                throw new NotFoundException(nameof(OrderItem), itemGroup.ProductAttributeOptionId);
            }

            var returnOrderCount = itemGroup.OrderItems
                .Sum(orderItem => orderItem.Quantity);
            var orderCount = orderItemDb.Quantity;
            var returnOrderDbsCount = returnOrderItemDbs
                .Where(c => c.ReturnOrderItemGroup.ProductAttributeOptionId == itemGroup.ProductAttributeOptionId)
                .Sum(c => c.Quantity);
            var orderCountAfterCheck = orderCount - returnOrderDbsCount;
            //todo: پاک کردن false
            if (returnOrderCount > orderCountAfterCheck && false)
            {
                throw new BadCommandException("مرجوعی سفارش");
            }


            if (itemGroup.OrderItems.Any(orderItem => orderItem.ReturnOrderItemDocuments.Count <= 0))
            {
                throw new BadCommandException("به دلیل ناموجود بودن مستندات");
            }
        }

        if (orderDb.PaymentMethods.OrderBy(c => c.Created).LastOrDefault() is OnlinePayment && request.CustomerAccountInfo == null)
        {
            throw new BadCommandException(" به دلیل عدم ارائه اطلاعات حساب کاربر با توجه به شیوه پرداخت ");
        }

        var returnOrder = new ReturnOrder()
        {
            OrderId = request.OrderId,
            Order = orderDb,
            CostRefundType = CostRefundType.NotDetermined,
            ReturnOrderTransportationType = ReturnOrderTransportationType.NotDetermined,
            Number = orderDb.Number + "-0",
            CustomerAccountInfo = request.CustomerAccountInfo,
        };


        var returnOrderItemGroups = new List<ReturnOrderItemGroup>();
        foreach (var itemGroup in request.ItemGroups)
        {
            var orderItemDb = orderDb.Items.SingleOrDefault(c => c.ProductAttributeOptionId == itemGroup.ProductAttributeOptionId);
            if (orderItemDb == null)
            {
                throw new NotFoundException(nameof(OrderItem), itemGroup.ProductAttributeOptionId);
            }

            var totalDiscountPercent = orderItemDb.DiscountPercentForProduct + orderDb.DiscountPercentForRole;
            var returnOrderItems = new List<ReturnOrderItem>();
            foreach (var item in itemGroup.OrderItems)
            {
                if (item.ReturnOrderReason.CustomerType != null && item.ReturnOrderReason.OrganizationType != null)
                {
                    throw new BadCommandException("با این اطلاعات");
                }
                var returnOrderItem = new ReturnOrderItem
                {
                    Quantity = item.Quantity,
                    IsAccepted = true,
                    ReturnOrderReason = new ReturnOrderReason()
                    {
                        Details = item.ReturnOrderReason.Details,
                    }
                };
                returnOrderItem.SetTotal(orderItemDb.Price);
                returnOrderItem.ReturnOrderReason.SetReturnOrderReasonType((ReturnOrderCustomerReasonType?)item.ReturnOrderReason.CustomerType, (ReturnOrderOrganizationReasonType?)item.ReturnOrderReason.OrganizationType);
                var returnOrderItemDocuments = new List<ReturnOrderItemDocument>();
                foreach (var documentCommand in item.ReturnOrderItemDocuments)
                {
                    var document = new ReturnOrderItemDocument();
                    document.Description = documentCommand.Description;

                    await _fileStore.SaveStoredFile(
                        documentCommand.Image,
                        FileCategory.ReturnOrderDocument.ToString(),
                        FileCategory.ReturnOrderDocument,
                        null,
                        false,
                        cancellationToken);

                    document.Image = documentCommand.Image;
                    returnOrderItemDocuments.Add(document);
                }
                returnOrderItem.ReturnOrderItemDocuments = returnOrderItemDocuments;
                returnOrderItems.Add(returnOrderItem);

            }

            var returnOrderItemGroup = new ReturnOrderItemGroup
            {
                Price = orderItemDb.Price,
                TotalDiscountPercent = totalDiscountPercent,
                ProductLocalizedName = orderItemDb.ProductLocalizedName,
                ProductName = orderItemDb.ProductName,
                ProductNo = orderItemDb.ProductNo,
                ProductAttributeOptionId = itemGroup.ProductAttributeOptionId,
                OptionValues = orderItemDb.OptionValues.Select(c =>
                    new ReturnOrderItemGroupProductAttributeOptionValue() { Name = c.Name, Value = c.Value }).ToList(),
                ReturnOrderItems = returnOrderItems
            };
            returnOrderItemGroup.SetTotalQuantity();
            returnOrderItemGroups.Add(returnOrderItemGroup);
        }

        returnOrder.ItemGroups = returnOrderItemGroups;
        returnOrder.SetDiscountTotal();
        returnOrder.SetTaxTotal();
        returnOrder.SetDeliveryTotal();

        returnOrder.SetQuantity();
        returnOrder.SetSubtotal();
        returnOrder.SetTotal();


        _createReturnOrderHelper.CheckRequestAndDatabase(request, returnOrder);

        returnOrder.ReturnOrderStateHistory.Add(new PendingForRegisterState()
        {
            ReturnOrderStatus = ReturnOrderStatus.PendingForRegister,
            Details = "درخواست ثبت سفارش بازگشت دریافت شد"
        });

        returnOrder.Register(request.Details);

        //todo
        returnOrder.Token = orderDb.Token + 1;

        await _context.ReturnOrders.AddAsync(returnOrder, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        //await _smsService.SendSms(orderDb.PhoneNumber, ".ثبت شد " + returnOrder.Number + "سفارش بازگشت شما به شماره ");

        return returnOrder.Id;
    }
}

