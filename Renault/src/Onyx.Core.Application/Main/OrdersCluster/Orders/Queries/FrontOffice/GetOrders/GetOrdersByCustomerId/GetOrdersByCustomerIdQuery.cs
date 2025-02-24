using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrders.GetOrdersByCustomerId;

public record GetOrdersByCustomerIdQuery(Guid CustomerId) : IRequest<List<OrderByCustomerIdDto>>;

public class GetOrdersByCustomerIdQueryHandler : IRequestHandler<GetOrdersByCustomerIdQuery, List<OrderByCustomerIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrdersByCustomerIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrderByCustomerIdDto>> Handle(GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .Where(x => x.CustomerId == request.CustomerId)
            .OrderBy(x => x.Number)
            .ProjectTo<OrderByCustomerIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var orderDto in orders)
        {
            var currentOrderState = orderDto.OrderStateHistory.OrderBy(e => e.Created).Last();
            orderDto.CurrentOrderStatus = currentOrderState.OrderStatus;
            orderDto.CurrentOrderStatusDetails = currentOrderState.Details;
            orderDto.OrderStateHistory = orderDto.OrderStateHistory
                .Where(state => state.OrderStatus != OrderStatus.OrderRegistered && state.OrderStatus != OrderStatus.PendingForRegister)
                .Concat(orderDto.OrderStateHistory
                    .Where(state => state.OrderStatus == OrderStatus.PendingForRegister)
                    .OrderByDescending(state => state.Created)
                    .Take(1))
                .Concat(orderDto.OrderStateHistory
                    .Where(state => state.OrderStatus == OrderStatus.OrderRegistered)
                    .OrderByDescending(state => state.Created)
                    .Take(1))
                .ToList();
        }

        return orders;
    }
}
