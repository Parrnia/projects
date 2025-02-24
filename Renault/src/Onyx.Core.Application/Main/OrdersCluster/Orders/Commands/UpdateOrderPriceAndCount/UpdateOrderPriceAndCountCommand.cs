using DocumentFormat.OpenXml.Vml.Office;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Services.SevenSoftServices;
using Onyx.Application.Settings;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.UpdateOrderPriceAndCount;

public record UpdateOrderPriceAndCountCommand : IRequest<Unit>
{
    public int OrderId { get; init; }
    public Guid CustomerId { get; set; }
};

public class UpdateOrderPriceAndCountCommandHandler : IRequestHandler<UpdateOrderPriceAndCountCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICreateOrderHelper _createOrderHelper;
    private readonly ISevenSoftService _sevenSoftService;
    private readonly ISmsService _smsService;
    private readonly ApplicationSettings _applicationSettings;


    public UpdateOrderPriceAndCountCommandHandler(IApplicationDbContext context,
        ICreateOrderHelper createOrderHelper,
        ISevenSoftService sevenSoftService,
        ISmsService smsService,
        IOptions<ApplicationSettings> applicationSettings)
    {
        _context = context;
        _createOrderHelper = createOrderHelper;
        _sevenSoftService = sevenSoftService;
        _smsService = smsService;
        _applicationSettings = applicationSettings.Value;
    }

    public async Task<Unit> Handle(UpdateOrderPriceAndCountCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(c => c.PaymentMethods)
            .Include(c => c.OrderStateHistory)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.AttributeOptions).ThenInclude(c => c.OptionValues)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.ColorOption).ThenInclude(e => e.Values)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.MaterialOption).ThenInclude(e => e.Values)
            .Include(e => e.Items)
            .Include(c => c.Totals)
            .SingleOrDefaultAsync(e => e.Id == request.OrderId, cancellationToken);

        if (order == null)
        {
            throw new NotFoundException(nameof(Order), request.OrderId);
        }

        if (order.CustomerId != request.CustomerId)
        {
            throw new OrderException("شما دسترسی لازم را ندارید");
        }

        if (order.GetCurrentOrderState().OrderStatus != OrderStatus.PendingForRegister)
        {
            throw new OrderException("به روزرسانی این سفارش امکان پذیر نیست");
        }

        var products = await _createOrderHelper.GetProducts(order.Items.Select(c => c.ProductAttributeOption.ProductId).ToList(), cancellationToken);
        
        order.Items = await _createOrderHelper.CheckAndSyncWithSevenAndUpdateOrder(products, order.Items.ToList(), order.CustomerTypeEnum, cancellationToken);

        order.SetQuantity();
        order.SetSubtotal();
        order.SetTaxTotal();
        order.SetDiscountOnProductTotal();
        order.SetDiscountOnCustomerTypeTotal();
        order.SetDeliveryTotal();
        order.SetTotal();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

}
