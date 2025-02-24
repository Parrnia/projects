using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrder.GetOrderByNumber;

public record GetOrderByNumberQuery() : IRequest<OrderByNumberDto?>
{
    public string Number { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
}

public class GetOrderByNumberQueryHandler : IRequestHandler<GetOrderByNumberQuery, OrderByNumberDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderByNumberQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OrderByNumberDto?> Handle(GetOrderByNumberQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .ProjectTo<OrderByNumberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Number == request.Number && x.PhoneNumber == request.PhoneNumber, cancellationToken);
        if (order == null)
        {
            throw new NotFoundException("سفارش مورد نظر یافت نشد");
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
