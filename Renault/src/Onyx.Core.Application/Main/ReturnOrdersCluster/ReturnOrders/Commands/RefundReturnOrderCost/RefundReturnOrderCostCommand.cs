using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReturnOrderModels;
using Onyx.Domain.Entities.ReturnOrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.RefundReturnOrderCost;
public record RefundReturnOrderCostCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Details { get; init; } = null!;
    public CostRefundType CostRefundType { get; init; }
}

public class RefundReturnOrderCostCommandHandler : IRequestHandler<RefundReturnOrderCostCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;

    public RefundReturnOrderCostCommandHandler(IApplicationDbContext context, ISmsService smsService)
    {
        _context = context;
        _smsService = smsService;
    }

    public async Task<Unit> Handle(RefundReturnOrderCostCommand request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .Include(c => c.Order)
            .ThenInclude(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .Include(c => c.Order)
            .ThenInclude(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.AttributeOptions).ThenInclude(c => c.OptionValues)
            .Include(c => c.Order)
            .ThenInclude(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.ColorOption).ThenInclude(e => e.Values)
            .Include(c => c.Order)
            .ThenInclude(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.MaterialOption).ThenInclude(e => e.Values)
            .Include(c => c.Order)
            .ThenInclude(e => e.Items)
            .Include(c => c.Order)
            .ThenInclude(e => e.Customer)
            .Include(c => c.ReturnOrderStateHistory)
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (returnOrder == null)
        {
            throw new NotFoundException(nameof(ReturnOrder), request.Id);
        }


        //var sevenCancelResult = await _sevenSoftService.CancelSevenSoftOrder(entity.Token, cancellationToken, false);
        //if (!sevenCancelResult)
        //{
        //    throw new SevenException("ارتباط با سرور دچار مشکل شده است، لطفا مدتی بعد دوباره تلاش کنید");
        //}

        returnOrder.CostRefundType = returnOrder.CostRefundType;
        returnOrder.RefundCost(request.Details);
        await _context.SaveChangesAsync(cancellationToken);

        await _smsService.SendSms(returnOrder.Order.PhoneNumber, ".تایید شد " + returnOrder.Number + "سفارش بازگشت شما به شماره ");

        return Unit.Value;
    }
}
