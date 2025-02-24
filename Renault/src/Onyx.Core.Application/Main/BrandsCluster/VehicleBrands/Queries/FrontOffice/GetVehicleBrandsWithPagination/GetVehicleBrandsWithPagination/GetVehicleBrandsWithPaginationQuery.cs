using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetVehicleBrandsWithPagination.GetVehicleBrandsWithPagination;
public record GetVehicleBrandsWithPaginationQuery : IRequest<PaginatedList<VehicleBrandWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetVehicleBrandsWithPaginationQueryHandler : IRequestHandler<GetVehicleBrandsWithPaginationQuery, PaginatedList<VehicleBrandWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVehicleBrandsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<VehicleBrandWithPaginationDto>> Handle(GetVehicleBrandsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.VehicleBrands
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<VehicleBrandWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
