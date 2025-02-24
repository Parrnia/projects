using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.FrontOffice.GetKinds.GetKindsByModelId;

public record GetKindsByModelIdQuery(int ModelId) : IRequest<List<KindByModelIdDto>>;

public class GetKindsByModelIdQueryHandler : IRequestHandler<GetKindsByModelIdQuery, List<KindByModelIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetKindsByModelIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<KindByModelIdDto>> Handle(GetKindsByModelIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Kinds
            .Where(x => x.ModelId == request.ModelId && x.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<KindByModelIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
