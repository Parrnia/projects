using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.OrderTotals.Commands.CreateOrderTotal;
public record CreateOrderTotalCommand : IRequest<int>
{
    public string Title { get; init; } = null!;
    public decimal Price { get; init; }
    public int Type { get; init; }
    public int OrderId { get; init; }
}

public class CreateOrderTotalCommandHandler : IRequestHandler<CreateOrderTotalCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateOrderTotalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateOrderTotalCommand request, CancellationToken cancellationToken)
    {
        var entity = new OrderTotal()
        {
            Title = request.Title,
            Price = request.Price,
            Type = (OrderTotalType) request.Type,
            OrderId = request.OrderId
        };

        

        _context.OrderTotals.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
