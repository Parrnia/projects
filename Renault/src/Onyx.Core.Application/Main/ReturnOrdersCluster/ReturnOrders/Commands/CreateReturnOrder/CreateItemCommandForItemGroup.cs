using MediatR;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReturnOrderModels;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.CreateReturnOrder;

public record CreateItemCommandForItemGroup : IRequest<int>
{
    public double Quantity { get; init; }
    public CreateReasonCommandForItem ReturnOrderReason { get; init; } = null!;
    public List<DocumentCommandForItem> ReturnOrderItemDocuments { get; init; } = new List<DocumentCommandForItem>();

}