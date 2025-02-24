using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ReturnOrdersCluster;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.UpdateReturnOrder;
public record UpdateReturnOrderCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string PhoneNumber { get; init; } = null!;
    public string CustomerFirstName { get; set; } = null!;
    public string CustomerLastName { get; set; } = null!;
}

public class UpdateReturnOrderCommandHandler : IRequestHandler<UpdateReturnOrderCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;

    public UpdateReturnOrderCommandHandler(IApplicationDbContext context, ISmsService smsService)
    {
        _context = context;
        _smsService = smsService;
    }

    public async Task<Unit> Handle(UpdateReturnOrderCommand request, CancellationToken cancellationToken)
    {
        var returnOrder = await _context.ReturnOrders
            .Include(c => c.Order)
            .Include(c => c.ReturnOrderStateHistory)
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (returnOrder == null)
        {
            throw new NotFoundException(nameof(ReturnOrder), request.Id);
        }

        returnOrder.Order.PhoneNumber = request.PhoneNumber;
        returnOrder.Order.CustomerFirstName = request.CustomerFirstName;
        returnOrder.Order.CustomerLastName = request.CustomerLastName;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
