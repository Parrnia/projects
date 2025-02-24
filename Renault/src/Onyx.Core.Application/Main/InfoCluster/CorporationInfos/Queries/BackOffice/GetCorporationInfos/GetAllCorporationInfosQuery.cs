using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Queries.BackOffice.GetCorporationInfos;

public record GetAllCorporationInfosQuery : IRequest<List<CorporationInfoDto>>;

public class GetAllCorporationInfosQueryHandler : IRequestHandler<GetAllCorporationInfosQuery, List<CorporationInfoDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCorporationInfosQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CorporationInfoDto>> Handle(GetAllCorporationInfosQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.CorporationInfos
            .ProjectTo<CorporationInfoDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return result;
    }
}
