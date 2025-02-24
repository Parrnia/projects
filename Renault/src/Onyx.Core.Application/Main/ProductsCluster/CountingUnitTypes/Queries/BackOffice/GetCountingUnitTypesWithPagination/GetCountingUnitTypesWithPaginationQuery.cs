using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.BackOffice.GetCountingUnitTypesWithPagination;
public record GetCountingUnitTypesWithPaginationQuery : IRequest<PaginatedList<CountingUnitTypeDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetCountingUnitTypesWithPaginationQueryHandler : IRequestHandler<GetCountingUnitTypesWithPaginationQuery, PaginatedList<CountingUnitTypeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCountingUnitTypesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CountingUnitTypeDto>> Handle(GetCountingUnitTypesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.CountingUnitTypes.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<CountingUnitTypeDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
