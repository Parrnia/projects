using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModelsWithPagination.GetModelsWithPagination;
public record GetModelsWithPaginationQuery : IRequest<PaginatedList<ModelWithPaginationDto>>
{
    public int? FamilyId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetModelsWithPaginationQueryHandler : IRequestHandler<GetModelsWithPaginationQuery, PaginatedList<ModelWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetModelsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ModelWithPaginationDto>> Handle(GetModelsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.Models
                .Where(x => x.FamilyId == request.FamilyId && x.IsActive)
                .OrderBy(x => x.Name)
                .ProjectTo<ModelWithPaginationDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        foreach (var modelDto in res.Items)
        {
            var list = modelDto.Kinds.Where(c => c.IsActive).ToList();
            modelDto.Kinds = list;
        }
        return res;
    }
}

