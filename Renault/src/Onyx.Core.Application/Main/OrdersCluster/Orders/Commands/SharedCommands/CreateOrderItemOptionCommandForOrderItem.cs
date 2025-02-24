using MediatR;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.SharedCommands;

public record CreateOrderItemOptionCommandForOrderItem : IRequest<int>
{
    public string Name { get; init; } = null!;
    public string Value { get; init; } = null!;
}