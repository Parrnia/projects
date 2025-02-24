using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrdersWithPagination.GetOrdersByCustomerIdWithPagination;
public record GetOrdersByCustomerIdWithPaginationQuery : IRequest<PaginatedList<OrderByCustomerIdWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public Guid? CustomerId { get; set; }
}

public class GetOrdersByCustomerIdWithPaginationQueryHandler : IRequestHandler<GetOrdersByCustomerIdWithPaginationQuery, PaginatedList<OrderByCustomerIdWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrdersByCustomerIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<OrderByCustomerIdWithPaginationDto>> Handle(GetOrdersByCustomerIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .Where(c => c.CustomerId == request.CustomerId)
            .OrderByDescending(x => x.CreatedAt)
            .ProjectTo<OrderByCustomerIdWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        foreach (var orderDto in orders.Items)
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
