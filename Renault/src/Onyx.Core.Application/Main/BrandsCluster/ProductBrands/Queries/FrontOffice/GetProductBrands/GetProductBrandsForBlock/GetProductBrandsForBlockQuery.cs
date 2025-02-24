using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.FrontOffice.GetProductBrands.GetProductBrandsForBlock;
public record GetProductBrandsForBlockQuery(int Limit) : IRequest<PaginatedList<ProductBrandForBlockDto>>;

public class GetAllProductBrandsForBlockQueryHandler : IRequestHandler<GetProductBrandsForBlockQuery, PaginatedList<ProductBrandForBlockDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductBrandsForBlockQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductBrandForBlockDto>> Handle(GetProductBrandsForBlockQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.ProductBrands
            .Where(c => c.IsActive)
            .OrderBy(c => c.LocalizedName)
            .ProjectTo<ProductBrandForBlockDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(1, request.Limit);

        return brands;
    }
}
