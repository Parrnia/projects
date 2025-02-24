using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.UnConfirmOrder;
public record UnConfirmOrderCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Details { get; init; } = null!;
}

public class UnConfirmOrderCommandHandler : IRequestHandler<UnConfirmOrderCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ISevenSoftService _sevenSoftService;
    private readonly ISmsService _smsService;

    public UnConfirmOrderCommandHandler(IApplicationDbContext context, ISevenSoftService sevenSoftService, ISmsService smsService)
    {
        _context = context;
        _sevenSoftService = sevenSoftService;
        _smsService = smsService;
    }

    public async Task<Unit> Handle(UnConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders
            .Include(c => c.PaymentMethods)
            .Include(c => c.OrderStateHistory)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.AttributeOptions)
            .ThenInclude(c => c.OptionValues)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.ColorOption).ThenInclude(e => e.Values)
            .Include(e => e.Items)
            .ThenInclude(c => c.ProductAttributeOption)
            .ThenInclude(c => c.Product)
            .ThenInclude(c => c.MaterialOption).ThenInclude(e => e.Values)
            .Include(e => e.Items)
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }
        entity.UnConfirmOrder(request.Details);
        //var sevenCancelResult = await _sevenSoftService.CancelSevenSoftOrder(entity.Token, cancellationToken);
        //if (!sevenCancelResult)
        //{
        //    throw new SevenException("ارتباط با سرور دچار مشکل شده است، لطفا مدتی بعد دوباره تلاش کنید");
        //}

        await _context.SaveChangesAsync(cancellationToken);

        await _smsService.SendSms(entity.PhoneNumber,   request.Details + " جزئیات عدم تایید سفارش: " +".تایید نشد " + entity.Token + " با شماره پیگیری " + entity.Number + "سفارش شما به شماره ");

        return Unit.Value;
    }
}
