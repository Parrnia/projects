using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.BackOffice.GetVehicles;

public record GetVehiclesByCustomerIdQuery(Guid CustomerId) : IRequest<List<VehicleByIdDto>>;

public class GetVehiclesByCustomerIdQueryHandler : IRequestHandler<GetVehiclesByCustomerIdQuery, List<VehicleByIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVehiclesByCustomerIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<VehicleByIdDto>> Handle(GetVehiclesByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _context.Vehicles
            .Where(x => x.CustomerId == request.CustomerId)
            .OrderBy(x => x.VinNumber)
            .ProjectTo<VehicleByIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return vehicles;
    }
}
