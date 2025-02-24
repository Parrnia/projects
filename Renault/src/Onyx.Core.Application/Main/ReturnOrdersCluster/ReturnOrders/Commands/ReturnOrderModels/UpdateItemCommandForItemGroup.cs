using MediatR;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReturnOrderModels;

public record UpdateItemCommandForItemGroup : IRequest<int>
{
    public int Id { get; init; }
    public double Quantity { get; init; }
    public bool IsAccepted { get; init; }
    public UpdateReasonCommandForItem ReturnOrderReason { get; init; } = null!;
}   