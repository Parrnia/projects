using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.BackOffice.GetBlockBanner;

public record GetBlockBannerByIdQuery(int Id) : IRequest<BlockBannerDto?>;

public class GetBlockBannerByIdQueryHandler : IRequestHandler<GetBlockBannerByIdQuery, BlockBannerDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBlockBannerByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BlockBannerDto?> Handle(GetBlockBannerByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.BlockBanners
            .ProjectTo<BlockBannerDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
