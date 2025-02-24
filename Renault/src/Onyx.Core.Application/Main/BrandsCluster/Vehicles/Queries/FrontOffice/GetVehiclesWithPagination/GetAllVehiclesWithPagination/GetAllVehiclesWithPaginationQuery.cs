using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Queries.FrontOffice.GetVehiclesWithPagination.GetAllVehiclesWithPagination;
public record GetAllVehiclesWithPaginationQuery : IRequest<PaginatedList<AllVehicleWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetVehiclesWithPaginationQueryHandler : IRequestHandler<GetAllVehiclesWithPaginationQuery, PaginatedList<AllVehicleWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVehiclesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AllVehicleWithPaginationDto>> Handle(GetAllVehiclesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Vehicles
            .OrderBy(x => x.Kind.LocalizedName)
            .ProjectTo<AllVehicleWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
