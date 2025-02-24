using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.BackOffice.GetKindsWithPagination;
public class GetKindsWithPaginationQuery : IRequest<PaginatedList<KindDto>>
{
    public int? ModelId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetKindsWithPaginationQueryHandler : IRequestHandler<GetKindsWithPaginationQuery, PaginatedList<KindDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetKindsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<KindDto>> Handle(GetKindsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Kinds.AsNoTracking()
                .Where(x => x.ModelId == request.ModelId)
                .OrderBy(x => x.Name)
                .ProjectTo<KindDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
