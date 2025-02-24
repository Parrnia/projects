using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.BackOffice.GetVehicleBrand;

public record GetVehicleBrandByIdQuery(int Id) : IRequest<VehicleBrandDto?>;

public class GetVehicleBrandByIdQueryHandler : IRequestHandler<GetVehicleBrandByIdQuery, VehicleBrandDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVehicleBrandByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<VehicleBrandDto?> Handle(GetVehicleBrandByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.VehicleBrands
            .ProjectTo<VehicleBrandDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
