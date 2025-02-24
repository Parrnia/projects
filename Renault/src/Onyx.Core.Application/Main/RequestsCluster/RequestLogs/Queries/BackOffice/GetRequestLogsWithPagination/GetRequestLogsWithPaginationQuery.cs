using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;

namespace Onyx.Application.Main.RequestsCluster.RequestLogs.Queries.BackOffice.GetRequestLogsWithPagination;
public record GetRequestLogsWithPaginationQuery : IRequest<FilteredRequestLogDto>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; } = null!;
    public string? SortDirection { get; init; } = null!;
    public string? SearchTerm { get; init; } = null!;
}

public class GetRequestLogsWithPaginationQueryHandler : IRequestHandler<GetRequestLogsWithPaginationQuery, FilteredRequestLogDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRequestLogsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FilteredRequestLogDto> Handle(GetRequestLogsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var requestLogs = _context.RequestLogs.OrderBy(c => c.Created).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            requestLogs = requestLogs.ApplySearch(request.SearchTerm);
        }

        if (!string.IsNullOrWhiteSpace(request.SortColumn) && !string.IsNullOrWhiteSpace(request.SortDirection))
        {
            requestLogs = requestLogs.ApplySorting(request.SortColumn, request.SortDirection);
        }

        var count = await requestLogs.CountAsync(cancellationToken);
        var skip = (request.PageNumber - 1) * request.PageSize;
        var dbRequestLogs = await requestLogs.Skip(skip).Take(request.PageSize)
            .ProjectToListAsync<RequestLogDto>(_mapper.ConfigurationProvider);
        return new FilteredRequestLogDto()
        {
            RequestLogs = dbRequestLogs,
            Count = count
        };
    }
}
