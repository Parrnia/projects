using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.BackOffice.GetCountingUnits;
public record GetAllCountingUnitsDropDownQuery : IRequest<List<AllCountingUnitDropDownDto>>;

public class GetAllCountingUnitsDropDownQueryHandler : IRequestHandler<GetAllCountingUnitsDropDownQuery, List<AllCountingUnitDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCountingUnitsDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllCountingUnitDropDownDto>> Handle(GetAllCountingUnitsDropDownQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.CountingUnits
            .OrderBy(x => x.Name)
            .ProjectTo<AllCountingUnitDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return result;
    }
}
