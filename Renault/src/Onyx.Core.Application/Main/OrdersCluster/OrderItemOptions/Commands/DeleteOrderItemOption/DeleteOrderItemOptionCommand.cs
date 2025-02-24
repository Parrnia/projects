using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.OrderItemOptions.Commands.DeleteOrderItemOption;
public record DeleteOrderItemOptionCommand(int Id) : IRequest<Unit>;

public class DeleteOrderItemOptionCommandHandler : IRequestHandler<DeleteOrderItemOptionCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteOrderItemOptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteOrderItemOptionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.OrderItemOptions
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OrderItemOption), request.Id);
        }

        _context.OrderItemOptions.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}