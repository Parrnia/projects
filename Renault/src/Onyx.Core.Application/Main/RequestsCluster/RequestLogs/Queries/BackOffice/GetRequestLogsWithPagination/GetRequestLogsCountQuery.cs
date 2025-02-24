using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.RequestsCluster.RequestLogs.Queries.BackOffice.GetRequestLogsWithPagination;
public record GetRequestLogsCountQuery : IRequest<int>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetRequestLogsCountQueryHandler : IRequestHandler<GetRequestLogsCountQuery, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRequestLogsCountQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(GetRequestLogsCountQuery request, CancellationToken cancellationToken)
    {
        return await _context.RequestLogs.CountAsync(cancellationToken);
    }
}
