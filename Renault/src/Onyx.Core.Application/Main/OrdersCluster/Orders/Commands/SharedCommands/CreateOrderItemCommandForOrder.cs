using MediatR;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.SharedCommands;

public record CreateOrderItemCommandForOrder : IRequest<int>
{
    public double Quantity { get; init; }
    public int ProductId { get; init; }
    public int ProductAttributeOptionId { get; init; }
    public IList<CreateOrderItemOptionCommandForOrderItem> OrderItemOptions { get; set; } = new List<CreateOrderItemOptionCommandForOrderItem>();
}