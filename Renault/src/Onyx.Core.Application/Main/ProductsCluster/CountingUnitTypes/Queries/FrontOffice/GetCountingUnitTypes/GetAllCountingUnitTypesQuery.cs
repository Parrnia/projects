using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.FrontOffice.GetCountingUnitTypes;
public record GetAllCountingUnitTypesQuery : IRequest<List<AllCountingUnitTypeDto>>;

public class GetAllCountingUnitTypesQueryHandler : IRequestHandler<GetAllCountingUnitTypesQuery, List<AllCountingUnitTypeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCountingUnitTypesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllCountingUnitTypeDto>> Handle(GetAllCountingUnitTypesQuery request, CancellationToken cancellationToken)
    {
        return await _context.CountingUnitTypes.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllCountingUnitTypeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
