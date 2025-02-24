using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.FrontOffice.GetKind.GetKindById;

public record GetKindByIdQuery(int Id) : IRequest<KindByIdDto?>;

public class GetKindByIdQueryHandler : IRequestHandler<GetKindByIdQuery, KindByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetKindByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<KindByIdDto?> Handle(GetKindByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Kinds
            .ProjectTo<KindByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
