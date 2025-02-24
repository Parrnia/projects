using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetVehicleBrand.GetVehicleBrandById;

public record GetVehicleBrandByIdQuery(int Id) : IRequest<VehicleBrandByIdDto?>;

public class GetVehicleBrandByIdQueryHandler : IRequestHandler<GetVehicleBrandByIdQuery, VehicleBrandByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVehicleBrandByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<VehicleBrandByIdDto?> Handle(GetVehicleBrandByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.VehicleBrands
            .ProjectTo<VehicleBrandByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
