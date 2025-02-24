using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrdersWithPagination;
public record GetReturnOrdersByOrderIdWithPaginationQuery : IRequest<PaginatedList<ReturnOrderDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int OrderId { get; init; }
}

public class GetReturnOrdersByOrderIdWithPaginationQueryHandler : IRequestHandler<GetReturnOrdersByOrderIdWithPaginationQuery, PaginatedList<ReturnOrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrdersByOrderIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReturnOrderDto>> Handle(GetReturnOrdersByOrderIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReturnOrders
            .Where(c => c.OrderId == request.OrderId)
            .OrderBy(x => x.CreatedAt)
            .ProjectTo<ReturnOrderDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
