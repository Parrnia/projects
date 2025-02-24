using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.LayoutsCluster.Carousels.Queries.BackOffice.GetCarouselsWithPagination;
public record GetCarouselsWithPaginationQuery : IRequest<PaginatedList<CarouselDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetCarouselsWithPaginationQueryHandler : IRequestHandler<GetCarouselsWithPaginationQuery, PaginatedList<CarouselDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCarouselsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CarouselDto>> Handle(GetCarouselsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Carousels
            .Where(c => c.IsActive)
            .OrderBy(c => c.Order)
            .ProjectTo<CarouselDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        return result;
    }
}
