using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.BackOffice.GetKinds;
public record GetAllKindsQuery : IRequest<List<KindDto>>;

public class GetAllKindsQueryHandler : IRequestHandler<GetAllKindsQuery, List<KindDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllKindsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<KindDto>> Handle(GetAllKindsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Kinds.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<KindDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
