using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.BackOffice.GetKinds;

public record GetKindsByModelIdQuery(int ModelId) : IRequest<List<KindDto>>;

public class GetKindsByModelIdQueryHandler : IRequestHandler<GetKindsByModelIdQuery, List<KindDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetKindsByModelIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<KindDto>> Handle(GetKindsByModelIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Kinds.AsNoTracking()
            .Where(x => x.ModelId == request.ModelId)
            .OrderBy(x => x.Name)
            .ProjectTo<KindDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
