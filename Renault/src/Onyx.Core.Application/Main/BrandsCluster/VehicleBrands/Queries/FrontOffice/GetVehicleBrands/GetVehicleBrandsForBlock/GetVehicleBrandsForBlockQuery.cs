using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetVehicleBrands.GetVehicleBrandsForBlock;
public record GetVehicleBrandsForBlockQuery(int Limit) : IRequest<PaginatedList<VehicleBrandForBlockDto>>;

public class GetAllVehicleBrandsForBlockQueryHandler : IRequestHandler<GetVehicleBrandsForBlockQuery, PaginatedList<VehicleBrandForBlockDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllVehicleBrandsForBlockQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<VehicleBrandForBlockDto>> Handle(GetVehicleBrandsForBlockQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.VehicleBrands
            .Where(c => c.IsActive)
            .OrderBy(c => c.LocalizedName)
            .ProjectTo<VehicleBrandForBlockDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(1, request.Limit);

        return brands;
    }
}
