using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.BackOffice.GetVehicles;
public record GetAllVehiclesQuery : IRequest<List<VehicleDto>>;

public class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, List<VehicleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllVehiclesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<VehicleDto>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _context.Vehicles
            .OrderBy(x => x.Kind.LocalizedName)
            .ProjectTo<VehicleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return vehicles;
    }
}
