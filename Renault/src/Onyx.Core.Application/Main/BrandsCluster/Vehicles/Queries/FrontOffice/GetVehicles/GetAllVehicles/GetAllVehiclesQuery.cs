using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.FrontOffice.GetVehicles.GetAllVehicles;
public record GetAllVehiclesQuery : IRequest<List<AllVehicleDto>>;

public class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, List<AllVehicleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllVehiclesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllVehicleDto>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.Vehicles
            .OrderBy(x => x.Kind.LocalizedName)
            .ProjectTo<AllVehicleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return brands;
    }
}
