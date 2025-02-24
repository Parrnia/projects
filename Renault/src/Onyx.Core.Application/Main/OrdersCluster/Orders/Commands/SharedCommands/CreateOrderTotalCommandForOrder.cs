using MediatR;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.SharedCommands;

public record CreateOrderTotalCommandForOrder : IRequest<int>
{
    public string Title { get; init; } = null!;
    public decimal Price { get; init; }
    public int Type { get; init; }
}