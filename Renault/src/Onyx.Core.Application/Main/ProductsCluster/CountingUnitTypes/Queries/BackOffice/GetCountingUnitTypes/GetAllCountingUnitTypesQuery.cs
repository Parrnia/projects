using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.BackOffice.GetCountingUnitTypes;
public record GetAllCountingUnitTypesQuery : IRequest<List<CountingUnitTypeDto>>;

public class GetAllCountingUnitTypesQueryHandler : IRequestHandler<GetAllCountingUnitTypesQuery, List<CountingUnitTypeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCountingUnitTypesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CountingUnitTypeDto>> Handle(GetAllCountingUnitTypesQuery request, CancellationToken cancellationToken)
    {
        return await _context.CountingUnitTypes.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<CountingUnitTypeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
