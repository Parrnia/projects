using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.BackOffice.GetProductBrandsWithPagination;
public record GetProductProductBrandsWithPaginationQuery : IRequest<PaginatedList<ProductBrandDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProductProductBrandsWithPaginationQueryHandler : IRequestHandler<GetProductProductBrandsWithPaginationQuery, PaginatedList<ProductBrandDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductProductBrandsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductBrandDto>> Handle(GetProductProductBrandsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductBrands
            .OrderBy(x => x.Name)
            .ProjectTo<ProductBrandDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
