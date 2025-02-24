using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.BackOffice.GetFamilies;

public record GetFamiliesByBrandIdQuery(int BrandId) : IRequest<List<FamilyDto>>;

public class GetFamiliesByBrandIdQueryHandler : IRequestHandler<GetFamiliesByBrandIdQuery, List<FamilyDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFamiliesByBrandIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FamilyDto>> Handle(GetFamiliesByBrandIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Families.AsNoTracking()
            .Where(x => x.VehicleBrandId == request.BrandId)
            .OrderBy(x => x.Name)
            .ProjectTo<FamilyDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
