using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ConfirmAllReturnOrder;
public record ConfirmAllReturnOrderCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Details { get; init; } = null!;

}

public class ConfirmAllReturnOrderCommandHandler : IRequestHandler<ConfirmAllReturnOrderCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;
    private readonly ICreateReturnOrderHelper _createReturnOrderHelper;

    public ConfirmAllReturnOrderCommandHandler(IApplicationDbContext context, ISmsService smsService, ICreateReturnOrderHelper createReturnOrderHelper)
    {
        _context = context;
        _smsService = smsService;
        _createReturnOrderHelper = createReturnOrderHelper;
    }

    public async Task<Unit> Handle(ConfirmAllReturnOrderCommand request, CancellationToken cancellationToken)
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


        returnOrder.ConfirmAll(request.Details);
        await _context.SaveChangesAsync(cancellationToken);

        await _smsService.SendSms(returnOrder.Order.PhoneNumber, ".تایید شد " + returnOrder.Number + "همه کالاهای سفارش بازگشت شما به شماره ");

        return Unit.Value;
    }
}
