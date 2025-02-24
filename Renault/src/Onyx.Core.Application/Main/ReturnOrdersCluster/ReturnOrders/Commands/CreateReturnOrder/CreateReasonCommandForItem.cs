using MediatR;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.CreateReturnOrder;

public record CreateReasonCommandForItem : IRequest<int>
{
    public string Details { get; init; } = null!;
    public int? CustomerType { get; init; }
    public int? OrganizationType { get; init; }
}