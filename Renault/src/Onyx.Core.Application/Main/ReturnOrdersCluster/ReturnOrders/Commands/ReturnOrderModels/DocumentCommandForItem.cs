using MediatR;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReturnOrderModels;

public record DocumentCommandForItem : IRequest<int>
{
    public Guid Image { get; init; }
    public string Description { get; init; } = null!;
}