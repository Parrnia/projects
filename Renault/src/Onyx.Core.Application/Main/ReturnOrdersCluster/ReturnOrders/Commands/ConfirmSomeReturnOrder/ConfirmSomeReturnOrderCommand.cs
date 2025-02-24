using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReturnOrderModels;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ConfirmSomeReturnOrder;
public record ConfirmSomeReturnOrderCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Details { get; init; } = null!;
}

public class ConfirmSomeReturnOrderCommandHandler : IRequestHandler<ConfirmSomeReturnOrderCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;
    private readonly ICreateReturnOrderHelper _createReturnOrderHelper;

    public ConfirmSomeReturnOrderCommandHandler(IApplicationDbContext context, ISmsService smsService, ICreateReturnOrderHelper createReturnOrderHelper)
    {
        _context = context;
        _smsService = smsService;
        _createReturnOrderHelper = createReturnOrderHelper;
    }

    public async Task<Unit> Handle(ConfirmSomeReturnOrderCommand request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .Include(c => c.Order)
            .Include(c => c.Order)
            .ThenInclude(c => c.Items)
            .Include(c => c.ItemGroups)
            .Include(c => c.ItemGroups)
            .ThenInclude(c => c.ReturnOrderItems)
            .Include(c => c.ItemGroups)
            .ThenInclude(c => c.ProductAttributeOption)
            .Include(c => c.ReturnOrderStateHistory)
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        if (returnOrder == null)
        {
            throw new NotFoundException(nameof(ReturnOrder), request.Id);
        }

        var checkCountResult = returnOrder.CheckCount();
        if (!checkCountResult)
        {
            throw new BadCommandException("به علت مغایرت در تعداد");
        }

        var checkReturnOrderTotalsResult = returnOrder.CheckReturnOrderTotals();
        if (!checkReturnOrderTotalsResult)
        {
            throw new BadCommandException("به دلیل عدم وجود مستندات در یکی از هزینه های جانبی");
        }

        returnOrder.ConfirmSome(request.Details);
        await _context.SaveChangesAsync(cancellationToken);

        await _smsService.SendSms(returnOrder.Order.PhoneNumber, ".تغییر کرده است " + returnOrder.Number + "وضعیت سفارش بازگشت شما به شماره ");

        return Unit.Value;
    }
}
