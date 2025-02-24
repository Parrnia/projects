using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.FrontOffice.GetVehicles.GetVehiclesByKindId;

public record GetVehiclesByKindIdQuery(int KindId) : IRequest<List<VehicleByKindIdDto>>;

public class GetVehiclesByKindIdQueryHandler : IRequestHandler<GetVehiclesByKindIdQuery, List<VehicleByKindIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVehiclesByKindIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<VehicleByKindIdDto>> Handle(GetVehiclesByKindIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Vehicles
            .Where(x => x.KindId == request.KindId)
            .OrderBy(x => x.Kind.LocalizedName)
            .ProjectTo<VehicleByKindIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
