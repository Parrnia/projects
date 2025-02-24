using MediatR;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReturnOrderModels;

public record UpdateItemGroupCommandForReturnOrder : IRequest<int>
{
    public int Id { get; init; }
    public List<UpdateItemCommandForItemGroup> OrderItems { get; init; } = new List<UpdateItemCommandForItemGroup>();
}