using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.FrontOffice.GetProductStatusesWithPagination;
public record GetProductStatusesWithPaginationQuery : IRequest<PaginatedList<ProductStatusWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProductStatusesWithPaginationQueryHandler : IRequestHandler<GetProductStatusesWithPaginationQuery, PaginatedList<ProductStatusWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductStatusesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductStatusWithPaginationDto>> Handle(GetProductStatusesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductStatuses
            .OrderBy(x => x.Name)
            .ProjectTo<ProductStatusWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
