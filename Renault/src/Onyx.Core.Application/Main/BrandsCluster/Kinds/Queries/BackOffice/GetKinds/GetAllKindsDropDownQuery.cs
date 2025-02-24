using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.BackOffice.GetKinds;
public record GetAllKindsDropDownQuery : IRequest<List<AllKindDropDownDto>>;

public class GetAllKindsDropDownQueryHandler : IRequestHandler<GetAllKindsDropDownQuery, List<AllKindDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllKindsDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllKindDropDownDto>> Handle(GetAllKindsDropDownQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Kinds.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllKindDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return result;
    }
}
