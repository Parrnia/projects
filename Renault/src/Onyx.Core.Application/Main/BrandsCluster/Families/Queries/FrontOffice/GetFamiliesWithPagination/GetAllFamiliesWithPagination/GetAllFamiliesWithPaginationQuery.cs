using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.FrontOffice.GetFamiliesWithPagination.GetAllFamiliesWithPagination;
public record GetAllFamiliesWithPaginationQuery : IRequest<PaginatedList<AllFamilyWithPaginationDto>>
{
    public int? BrandId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFamiliesWithPaginationQueryHandler : IRequestHandler<GetAllFamiliesWithPaginationQuery, PaginatedList<AllFamilyWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFamiliesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AllFamilyWithPaginationDto>> Handle(GetAllFamiliesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.Families
            .Where(x => x.VehicleBrandId == request.BrandId && x.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<AllFamilyWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        foreach (var familyDto in res.Items)
        {
            var list = familyDto.Models.Where(c => c.IsActive).ToList();
            familyDto.Models = list;
        }

        return res;

    }


}
