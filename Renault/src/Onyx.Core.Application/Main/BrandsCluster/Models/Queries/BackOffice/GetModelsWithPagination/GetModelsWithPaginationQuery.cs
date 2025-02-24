using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.BackOffice.GetModelsWithPagination;
public record GetModelsWithPaginationQuery : IRequest<PaginatedList<ModelDto>>
{
    public int? FamilyId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetModelsWithPaginationQueryHandler : IRequestHandler<GetModelsWithPaginationQuery, PaginatedList<ModelDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetModelsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ModelDto>> Handle(GetModelsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Models.AsNoTracking()
                .Where(x => x.FamilyId == request.FamilyId)
                .OrderBy(x => x.Name)
                .ProjectTo<ModelDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}

