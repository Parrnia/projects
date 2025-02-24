using MediatR;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReturnOrderModels;

public record DocumentCommandForTotal : IRequest<int>
{
    public Guid Image { get; init; }
    public string Description { get; init; } = null!;
}