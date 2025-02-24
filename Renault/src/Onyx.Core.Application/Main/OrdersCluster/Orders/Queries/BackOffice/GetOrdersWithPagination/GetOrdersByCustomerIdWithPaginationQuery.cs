using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrdersWithPagination;
public record GetOrdersByCustomerIdWithPaginationQuery : IRequest<PaginatedList<OrderDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public Guid CustomerId { get; init; }
}

public class GetOrdersByCustomerIdWithPaginationQueryHandler : IRequestHandler<GetOrdersByCustomerIdWithPaginationQuery, PaginatedList<OrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrdersByCustomerIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<OrderDto>> Handle(GetOrdersByCustomerIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Where(c => c.CustomerId == request.CustomerId)
            .OrderBy(x => x.CreatedAt)
            .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
