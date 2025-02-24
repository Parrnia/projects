using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrdersWithPagination;
public record GetReturnOrdersByProductIdWithPaginationQuery : IRequest<PaginatedList<ReturnOrderDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int ProductId { get; init; }
}

public class GetReturnOrdersByProductIdWithPaginationQueryHandler : IRequestHandler<GetReturnOrdersByProductIdWithPaginationQuery, PaginatedList<ReturnOrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrdersByProductIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReturnOrderDto>> Handle(GetReturnOrdersByProductIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReturnOrders
            .Where(c => c.Order.Items.Any(e => e.ProductAttributeOption.ProductId == request.ProductId))
            .OrderBy(x => x.CreatedAt)
            .ProjectTo<ReturnOrderDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
