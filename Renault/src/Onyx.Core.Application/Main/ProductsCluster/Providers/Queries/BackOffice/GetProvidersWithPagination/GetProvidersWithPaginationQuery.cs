using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;

namespace Onyx.Application.Main.ProductsCluster.Providers.Queries.BackOffice.GetProvidersWithPagination;
public record GetProvidersWithPaginationQuery : IRequest<FilteredProviderDto>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; } = null!;
    public string? SortDirection { get; init; } = null!;
    public string? SearchTerm { get; init; } = null!;
}

public class GetProvidersWithPaginationQueryHandler : IRequestHandler<GetProvidersWithPaginationQuery, FilteredProviderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProvidersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FilteredProviderDto> Handle(GetProvidersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var orders = _context.Providers.OrderBy(c => c.Created).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            orders = orders.ApplySearch(request.SearchTerm);
        }

        if (!string.IsNullOrWhiteSpace(request.SortColumn) && !string.IsNullOrWhiteSpace(request.SortDirection))
        {
            orders = orders.ApplySorting(request.SortColumn, request.SortDirection);
        }

        var count = await orders.CountAsync(cancellationToken);
        var skip = (request.PageNumber - 1) * request.PageSize;
        var dbProviders = await orders.Skip(skip).Take(request.PageSize)
            .ProjectToListAsync<ProviderDto>(_mapper.ConfigurationProvider);
        return new FilteredProviderDto()
        {
            Providers = dbProviders,
            Count = count
        };
    }
}
