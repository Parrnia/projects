using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetAllVehicleBrandsForDropDown;
public record GetAllVehicleBrandsForDropDownQuery : IRequest<List<AllVehicleBrandForDropDownDto>>;

public class GetAllVehicleBrandsForDropDownQueryHandler : IRequestHandler<GetAllVehicleBrandsForDropDownQuery, List<AllVehicleBrandForDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllVehicleBrandsForDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllVehicleBrandForDropDownDto>> Handle(GetAllVehicleBrandsForDropDownQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.VehicleBrands
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<AllVehicleBrandForDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return brands;
    }
}