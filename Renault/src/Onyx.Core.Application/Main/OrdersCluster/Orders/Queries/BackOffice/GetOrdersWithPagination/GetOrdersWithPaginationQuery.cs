using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrdersWithPagination;
public record GetOrdersWithPaginationQuery : IRequest<FilteredOrderDto>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; } = null!;
    public string? SortDirection { get; init; } = null!;
    public string? SearchTerm { get; init; } = null!;
}

public class GetOrdersWithPaginationQueryHandler : IRequestHandler<GetOrdersWithPaginationQuery, FilteredOrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrdersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FilteredOrderDto> Handle(GetOrdersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var orders = _context.Orders.OrderBy(c => c.CreatedAt).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            orders = orders.ApplySearch(request.SearchTerm);
        }

        if (!string.IsNullOrWhiteSpace(request.SortColumn) && !string.IsNullOrWhiteSpace(request.SortDirection))
        {
            orders = orders.ApplySorting(request.SortColumn, request.SortDirection);
        }

        var count = await orders.CountAsync(cancellationToken);
        var skip = (request.PageNumber - 1) * request.PageSize;
        var dbOrders = await orders.Skip(skip).Take(request.PageSize)
            .ProjectToListAsync<OrderDto>(_mapper.ConfigurationProvider);
        return new FilteredOrderDto()
        {
            Orders = dbOrders,
            Count = count
        };
    }
}
