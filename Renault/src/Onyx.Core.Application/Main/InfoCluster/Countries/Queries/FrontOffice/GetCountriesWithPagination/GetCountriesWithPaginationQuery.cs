using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.InfoCluster.Countries.Queries.FrontOffice.GetCountriesWithPagination;
public record GetCountriesWithPaginationQuery : IRequest<PaginatedList<CountryWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetCountriesWithPaginationQueryHandler : IRequestHandler<GetCountriesWithPaginationQuery, PaginatedList<CountryWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCountriesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CountryWithPaginationDto>> Handle(GetCountriesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Countries.AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<CountryWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
