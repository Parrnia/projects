using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.FrontOffice.GetVehicles.GetVehiclesByCustomerId;

public record GetVehiclesByCustomerIdQuery(Guid CustomerId) : IRequest<List<VehicleByCustomerIdDto>>;

public class GetVehiclesByCustomerIdQueryHandler : IRequestHandler<GetVehiclesByCustomerIdQuery, List<VehicleByCustomerIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVehiclesByCustomerIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<VehicleByCustomerIdDto>> Handle(GetVehiclesByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _context.Vehicles
            .Where(x => x.CustomerId == request.CustomerId)
            .OrderBy(x => x.VinNumber)
            .ProjectTo<VehicleByCustomerIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return vehicles;
    }
}
