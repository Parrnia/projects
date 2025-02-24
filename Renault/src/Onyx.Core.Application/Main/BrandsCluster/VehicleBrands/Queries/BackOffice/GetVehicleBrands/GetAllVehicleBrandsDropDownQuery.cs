using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.BackOffice.GetVehicleBrands;
public record GetAllVehicleBrandsDropDownQuery : IRequest<List<AllVehicleBrandDropDownDto>>;

public class GetAllVehicleBrandsDropDownQueryHandler : IRequestHandler<GetAllVehicleBrandsDropDownQuery, List<AllVehicleBrandDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllVehicleBrandsDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllVehicleBrandDropDownDto>> Handle(GetAllVehicleBrandsDropDownQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.VehicleBrands
            .OrderBy(x => x.Name)
            .ProjectTo<AllVehicleBrandDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return brands;
    }
}
