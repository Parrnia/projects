using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.OrderItemOptions.Commands.UpdateOrderItemOption;
public record UpdateOrderItemOptionCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Value { get; init; } = null!;
    public int OrderItemId { get; init; }
}

public class UpdateOrderItemOptionCommandHandler : IRequestHandler<UpdateOrderItemOptionCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateOrderItemOptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateOrderItemOptionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.OrderItemOptions
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OrderItemOption), request.Id);
        }

        entity.Name = request.Name;
        entity.Value = request.Value;
        entity.OrderItemId = request.OrderItemId;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
