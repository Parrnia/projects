using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.BackOffice.GetBlockBannersWithPagination;
public record GetBlockBannersWithPaginationQuery : IRequest<PaginatedList<BlockBannerDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetBlockBannersWithPaginationQueryHandler : IRequestHandler<GetBlockBannersWithPaginationQuery, PaginatedList<BlockBannerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBlockBannersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<BlockBannerDto>> Handle(GetBlockBannersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.BlockBanners
            .OrderBy(c => c.Title)
            .ProjectTo<BlockBannerDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        return result;
    }
}
