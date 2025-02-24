using MediatR;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.CreateReturnOrder;

public record CreateItemGroupCommandForReturnOrder : IRequest<int>
{
    public int ProductAttributeOptionId { get; init; }
    public List<CreateItemCommandForItemGroup> OrderItems { get; init; } = new List<CreateItemCommandForItemGroup>();
}