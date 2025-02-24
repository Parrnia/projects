using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.FrontOffice.GetVehicle.GetVehicleByVinNumber;

public record GetVehicleByVinNumberQuery(string VinNumber) : IRequest<VehicleByVinNumberDto?>;

public class GetVehicleByVinNumberQueryHandler : IRequestHandler<GetVehicleByVinNumberQuery, VehicleByVinNumberDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVehicleByVinNumberQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<VehicleByVinNumberDto?> Handle(GetVehicleByVinNumberQuery request, CancellationToken cancellationToken)
    {
        return await _context.Vehicles
            .Where(c => c.VinNumber == request.VinNumber)
            .ProjectTo<VehicleByVinNumberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
