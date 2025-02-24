using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.BackOffice.GetFamiliesWithPagination;
public record GetFamiliesWithPaginationQuery : IRequest<PaginatedList<FamilyDto>>
{
    public int? BrandId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFamiliesWithPaginationQueryHandler : IRequestHandler<GetFamiliesWithPaginationQuery, PaginatedList<FamilyDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFamiliesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<FamilyDto>> Handle(GetFamiliesWithPaginationQuery request, CancellationToken cancellationToken)
    {

        return await _context.Families.AsNoTracking()
            .Where(x => x.VehicleBrandId == request.BrandId)
            .OrderBy(x => x.Name)
            .ProjectTo<FamilyDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

    }


}
