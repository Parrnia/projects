using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.BackOffice.GetVehicle;

public record GetVehicleByVinNumberQuery(string VinNumber) : IRequest<VehicleForVinDto?>;

public class GetVehicleByVinNumberQueryHandler : IRequestHandler<GetVehicleByVinNumberQuery, VehicleForVinDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVehicleByVinNumberQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<VehicleForVinDto?> Handle(GetVehicleByVinNumberQuery request, CancellationToken cancellationToken)
    {
        return await _context.Vehicles
            .Where(c => c.VinNumber == request.VinNumber)
            .ProjectTo<VehicleForVinDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
