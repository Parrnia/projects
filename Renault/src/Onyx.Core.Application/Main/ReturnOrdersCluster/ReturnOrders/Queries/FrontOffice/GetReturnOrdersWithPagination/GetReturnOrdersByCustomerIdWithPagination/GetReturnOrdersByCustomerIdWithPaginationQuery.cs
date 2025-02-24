using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrdersWithPagination.GetReturnOrdersByCustomerIdWithPagination;
public record GetReturnOrdersByCustomerIdWithPaginationQuery : IRequest<PaginatedList<ReturnOrderByCustomerIdWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public Guid CustomerId { get; set; }
}

public class GetReturnOrdersByCustomerIdWithPaginationQueryHandler : IRequestHandler<GetReturnOrdersByCustomerIdWithPaginationQuery, PaginatedList<ReturnOrderByCustomerIdWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrdersByCustomerIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReturnOrderByCustomerIdWithPaginationDto>> Handle(GetReturnOrdersByCustomerIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var returnOrders = await _context.ReturnOrders
            .Where(c => c.Order.CustomerId == request.CustomerId)
            .OrderByDescending(x => x.CreatedAt)
            .ProjectTo<ReturnOrderByCustomerIdWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);


        return returnOrders;
    }
}
