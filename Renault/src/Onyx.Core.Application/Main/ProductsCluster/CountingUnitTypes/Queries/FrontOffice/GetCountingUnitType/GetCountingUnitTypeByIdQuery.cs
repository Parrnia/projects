using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.FrontOffice.GetCountingUnitType;

public record GetCountingUnitTypeByIdQuery(int Id) : IRequest<CountingUnitTypeByIdDto?>;

public class GetCountingUnitTypeByIdQueryHandler : IRequestHandler<GetCountingUnitTypeByIdQuery, CountingUnitTypeByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCountingUnitTypeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CountingUnitTypeByIdDto?> Handle(GetCountingUnitTypeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.CountingUnitTypes
            .ProjectTo<CountingUnitTypeByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
