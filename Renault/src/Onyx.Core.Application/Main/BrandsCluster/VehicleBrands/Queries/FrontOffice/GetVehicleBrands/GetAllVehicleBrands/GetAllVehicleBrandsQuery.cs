using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetVehicleBrands.GetAllVehicleBrands;
public record GetAllVehicleBrandsQuery : IRequest<List<AllVehicleBrandDto>>;

public class GetAllVehicleBrandsQueryHandler : IRequestHandler<GetAllVehicleBrandsQuery, List<AllVehicleBrandDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllVehicleBrandsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllVehicleBrandDto>> Handle(GetAllVehicleBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.VehicleBrands
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<AllVehicleBrandDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return brands;
    }
}
