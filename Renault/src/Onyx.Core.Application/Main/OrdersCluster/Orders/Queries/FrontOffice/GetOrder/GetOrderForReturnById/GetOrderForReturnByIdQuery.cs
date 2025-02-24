using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrder.GetOrderForReturnById;

public record GetOrderForReturnByIdQuery(int Id, Guid? CustomerId) : IRequest<OrderForReturnByIdDto?>;

public class GetOrderForReturnByIdQueryHandler : IRequestHandler<GetOrderForReturnByIdQuery, OrderForReturnByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderForReturnByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OrderForReturnByIdDto?> Handle(GetOrderForReturnByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .ProjectTo<OrderForReturnByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        if (order == null)
        {
            return null;
        }
        if (request.CustomerId != null && order.CustomerId != request.CustomerId)
        {
            throw new ForbiddenAccessException("GetOrderForReturnByIdQuery");
        }
        var currentOrderState = order.OrderStateHistory.OrderBy(e => e.Created).Last();
        order.CurrentOrderStatus = currentOrderState.OrderStatus;
        order.CurrentOrderStatusDetails = currentOrderState.Details;
        order.OrderStateHistory = order.OrderStateHistory
            .Where(state => state.OrderStatus != OrderStatus.OrderRegistered && state.OrderStatus != OrderStatus.PendingForRegister)
            .Concat(order.OrderStateHistory
                .Where(state => state.OrderStatus == OrderStatus.PendingForRegister)
                .OrderByDescending(state => state.Created)
                .Take(1))
            .Concat(order.OrderStateHistory
                .Where(state => state.OrderStatus == OrderStatus.OrderRegistered)
                .OrderByDescending(state => state.Created)
                .Take(1))
            .ToList();
        return order;
    }
}
