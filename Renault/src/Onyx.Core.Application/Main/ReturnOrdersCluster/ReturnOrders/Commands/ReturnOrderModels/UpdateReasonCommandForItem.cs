using MediatR;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReturnOrderModels;

public record UpdateReasonCommandForItem : IRequest<int>
{
    public int Id { get; init; }
    public string Details { get; init; } = null!;
    public ReturnOrderCustomerReasonType? CustomerType { get; init; }
    public ReturnOrderOrganizationReasonType? OrganizationType { get; init; }
}