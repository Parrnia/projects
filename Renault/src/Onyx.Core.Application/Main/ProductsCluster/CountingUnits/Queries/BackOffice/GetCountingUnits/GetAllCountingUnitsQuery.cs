using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.BackOffice.GetCountingUnits;
public record GetAllCountingUnitsQuery : IRequest<List<CountingUnitDto>>;

public class GetAllCountingUnitsQueryHandler : IRequestHandler<GetAllCountingUnitsQuery, List<CountingUnitDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCountingUnitsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CountingUnitDto>> Handle(GetAllCountingUnitsQuery request, CancellationToken cancellationToken)
    {
        return await _context.CountingUnits.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<CountingUnitDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
