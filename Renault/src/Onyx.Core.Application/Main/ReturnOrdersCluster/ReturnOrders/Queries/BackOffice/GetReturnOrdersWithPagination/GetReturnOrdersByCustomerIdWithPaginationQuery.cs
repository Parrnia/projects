using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrdersWithPagination;
public record GetReturnOrdersByCustomerIdWithPaginationQuery : IRequest<PaginatedList<ReturnOrderDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public Guid CustomerId { get; init; }
}

public class GetReturnOrdersByCustomerIdWithPaginationQueryHandler : IRequestHandler<GetReturnOrdersByCustomerIdWithPaginationQuery, PaginatedList<ReturnOrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrdersByCustomerIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReturnOrderDto>> Handle(GetReturnOrdersByCustomerIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReturnOrders
            .Where(c => c.Order.CustomerId == request.CustomerId)
            .OrderBy(x => x.CreatedAt)
            .ProjectTo<ReturnOrderDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
