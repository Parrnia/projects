using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrder.GetOrderById;

public record GetOrderByIdQuery(int Id, Guid? CustomerId) : IRequest<OrderByIdDto?>;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OrderByIdDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .ProjectTo<OrderByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        if (order == null)
        {
            return null;
        }
        if (request.CustomerId != null && order.CustomerId != request.CustomerId)
        {
            throw new ForbiddenAccessException("GetOrderByIdQuery");
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
            .ToList().OrderBy(c => c.Created).ToList();

        return order;
    }
}
