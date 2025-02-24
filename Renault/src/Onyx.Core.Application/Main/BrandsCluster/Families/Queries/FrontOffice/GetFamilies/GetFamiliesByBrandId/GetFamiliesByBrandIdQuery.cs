using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.FrontOffice.GetFamilies.GetFamiliesByBrandId;

public record GetFamiliesByBrandIdQuery(int BrandId) : IRequest<List<FamilyByBrandIdDto>>;

public class GetFamiliesByBrandIdQueryHandler : IRequestHandler<GetFamiliesByBrandIdQuery, List<FamilyByBrandIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFamiliesByBrandIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FamilyByBrandIdDto>> Handle(GetFamiliesByBrandIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Families.AsNoTracking()
            .Where(x => x.VehicleBrandId == request.BrandId && x.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<FamilyByBrandIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
