using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.FrontOffice.GetCountingUnitsWithPagination;
public record GetCountingUnitsWithPaginationQuery : IRequest<PaginatedList<CountingUnitWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetCountingUnitsWithPaginationQueryHandler : IRequestHandler<GetCountingUnitsWithPaginationQuery, PaginatedList<CountingUnitWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCountingUnitsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CountingUnitWithPaginationDto>> Handle(GetCountingUnitsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.CountingUnits.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<CountingUnitWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
