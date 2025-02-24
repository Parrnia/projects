using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.BackOffice.GetVehicleBrands;
public record GetAllVehicleBrandsQuery : IRequest<List<VehicleBrandDto>>;

public class GetAllVehicleBrandsQueryHandler : IRequestHandler<GetAllVehicleBrandsQuery, List<VehicleBrandDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllVehicleBrandsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<VehicleBrandDto>> Handle(GetAllVehicleBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.VehicleBrands
            .OrderBy(x => x.Name)
            .ProjectTo<VehicleBrandDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return brands;
    }
}
