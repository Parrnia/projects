using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.BackOffice.GetVehicleBrandsWithPagination;
public record GetVehicleBrandsWithPaginationQuery : IRequest<PaginatedList<VehicleBrandDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetVehicleBrandsWithPaginationQueryHandler : IRequestHandler<GetVehicleBrandsWithPaginationQuery, PaginatedList<VehicleBrandDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVehicleBrandsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<VehicleBrandDto>> Handle(GetVehicleBrandsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.VehicleBrands
            .OrderBy(x => x.Name)
            .ProjectTo<VehicleBrandDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
