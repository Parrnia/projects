using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Queries.FrontOffice.GetCorporationInfo;

public record GetCorporationInfoQuery : IRequest<CorporationInfoDto?>;

public class GetCorporationInfoQueryHandler : IRequestHandler<GetCorporationInfoQuery, CorporationInfoDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCorporationInfoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CorporationInfoDto?> Handle(GetCorporationInfoQuery request, CancellationToken cancellationToken)
    {
        return await _context.CorporationInfos
            .ProjectTo<CorporationInfoDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c => c.IsDefault,cancellationToken: cancellationToken);
    }
}
