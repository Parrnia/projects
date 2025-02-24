using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.FrontOffice.GetProductBrandsWithPagination.GetProductBrandsWithPagination;
public record GetProductBrandsWithPaginationQuery : IRequest<PaginatedList<ProductBrandWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProductBrandsWithPaginationQueryHandler : IRequestHandler<GetProductBrandsWithPaginationQuery, PaginatedList<ProductBrandWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductBrandsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductBrandWithPaginationDto>> Handle(GetProductBrandsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.ProductBrands
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<ProductBrandWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        
        return brands;
    }
}
