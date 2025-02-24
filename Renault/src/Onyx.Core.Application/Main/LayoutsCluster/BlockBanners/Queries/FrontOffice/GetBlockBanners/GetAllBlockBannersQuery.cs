using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.FrontOffice.GetBlockBanners;
public record GetAllBlockBannersQuery : IRequest<List<BlockBannerDto>>;

public class GetAllBlockBannersQueryHandler : IRequestHandler<GetAllBlockBannersQuery, List<BlockBannerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllBlockBannersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BlockBannerDto>> Handle(GetAllBlockBannersQuery request, CancellationToken cancellationToken)
    {
        return await _context.BlockBanners
            .Where(c => c.IsActive)
            .OrderBy(x => x.Title)
            .ProjectTo<BlockBannerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
