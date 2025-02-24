using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.BackOffice.GetProductStatusesWithPagination;
public record GetProductStatusesWithPaginationQuery : IRequest<PaginatedList<ProductStatusDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProductStatusesWithPaginationQueryHandler : IRequestHandler<GetProductStatusesWithPaginationQuery, PaginatedList<ProductStatusDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductStatusesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductStatusDto>> Handle(GetProductStatusesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductStatuses.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<ProductStatusDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
