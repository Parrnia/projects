using MediatR;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReturnOrderModels;

public record TotalCommandForReturnOrder : IRequest<int>
{
    public string Title { get; init; } = null!;
    public decimal Price { get; init; }
    public ReturnOrderTotalType Type { get; init; }
    public ReturnOrderTotalApplyType ReturnOrderTotalApplyType { get; init; }
    public List<DocumentCommandForTotal> DocumentCommandForTotals { get; init; } = new List<DocumentCommandForTotal>();
}