using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.FrontOffice.GetCountingUnit;

public record GetCountingUnitByIdQuery(int Id) : IRequest<CountingUnitByIdDto?>;

public class GetCountingUnitByIdQueryHandler : IRequestHandler<GetCountingUnitByIdQuery, CountingUnitByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCountingUnitByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CountingUnitByIdDto?> Handle(GetCountingUnitByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.CountingUnits
            .ProjectTo<CountingUnitByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
