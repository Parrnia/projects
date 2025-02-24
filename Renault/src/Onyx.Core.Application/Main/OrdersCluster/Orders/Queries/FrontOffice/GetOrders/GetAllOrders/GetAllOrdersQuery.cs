using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrders.GetAllOrders;
public record GetAllOrdersQuery : IRequest<List<AllOrderDto>>;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<AllOrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllOrdersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllOrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .OrderBy(x => x.Number)
            .ProjectTo<AllOrderDto>(_mapper.ConfigurationProvider)
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
