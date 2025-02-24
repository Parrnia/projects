using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.ProductsCluster.Providers.Queries.FrontOffice.GetProvidersWithPagination;
public record GetProvidersWithPaginationQuery : IRequest<PaginatedList<ProviderWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProvidersWithPaginationQueryHandler : IRequestHandler<GetProvidersWithPaginationQuery, PaginatedList<ProviderWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProvidersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProviderWithPaginationDto>> Handle(GetProvidersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Providers.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<ProviderWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
