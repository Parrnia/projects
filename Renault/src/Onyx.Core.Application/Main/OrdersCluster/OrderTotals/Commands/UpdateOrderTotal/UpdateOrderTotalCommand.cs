using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.OrderTotals.Commands.UpdateOrderTotal;
public record UpdateOrderTotalCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public decimal Price { get; init; }
    public int Type { get; init; }
    public int OrderId { get; init; }
}

public class UpdateOrderTotalCommandHandler : IRequestHandler<UpdateOrderTotalCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateOrderTotalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateOrderTotalCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.OrderTotals
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OrderTotal), request.Id);
        }
        entity.Title = request.Title;
        entity.Price = request.Price;
        entity.Type = (OrderTotalType)request.Type;
        entity.OrderId = request.OrderId;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
