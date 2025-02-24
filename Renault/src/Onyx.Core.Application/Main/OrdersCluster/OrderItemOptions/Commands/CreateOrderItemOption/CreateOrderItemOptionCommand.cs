using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.OrderItemOptions.Commands.CreateOrderItemOption;
public record CreateOrderItemOptionCommand : IRequest<int>
{
    public string Name { get; init; } = null!;
    public string Value { get; init; } = null!;
    public int OrderItemId { get; init; }
}

public class CreateOrderItemOptionCommandHandler : IRequestHandler<CreateOrderItemOptionCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateOrderItemOptionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateOrderItemOptionCommand request, CancellationToken cancellationToken)
    {
        var entity = new OrderItemOption()
        {
            Name = request.Name,
            Value = request.Value,
            OrderItemId = request.OrderItemId
        };

        

        _context.OrderItemOptions.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
