using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.BackOffice.GetBlockBanners;
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
        var res =  await _context.BlockBanners
            .OrderBy(x => x.Title)
            .ProjectTo<BlockBannerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return res;
    }
}
