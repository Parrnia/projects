using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Queries.BackOffice.GetCorporationInfo;

public record GetCorporationInfoByIdQuery(int Id) : IRequest<CorporationInfoDto?>;

public class GetCorporationInfoByIdQueryHandler : IRequestHandler<GetCorporationInfoByIdQuery, CorporationInfoDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCorporationInfoByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CorporationInfoDto?> Handle(GetCorporationInfoByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.CorporationInfos
            .ProjectTo<CorporationInfoDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
