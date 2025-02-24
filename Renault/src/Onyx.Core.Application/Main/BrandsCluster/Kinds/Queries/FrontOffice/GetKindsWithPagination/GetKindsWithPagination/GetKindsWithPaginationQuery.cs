using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.FrontOffice.GetKindsWithPagination.GetKindsWithPagination;
public class GetKindsWithPaginationQuery : IRequest<PaginatedList<KindWithPaginationDto>>
{
    public int? ModelId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetKindsWithPaginationQueryHandler : IRequestHandler<GetKindsWithPaginationQuery, PaginatedList<KindWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetKindsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<KindWithPaginationDto>> Handle(GetKindsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.Kinds
                .Where(x => x.ModelId == request.ModelId && x.IsActive)
                .OrderBy(x => x.Name)
                .ProjectTo<KindWithPaginationDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        foreach (var allKindDto in res.Items)
        {
            var list1 = allKindDto.Products.Where(c => c.IsActive).ToList();
            var list2 = allKindDto.Vehicles.Where(c => c.IsActive).ToList();
            allKindDto.Products = list1;
            allKindDto.Vehicles = list2;
        }

        return res;
    }
}
