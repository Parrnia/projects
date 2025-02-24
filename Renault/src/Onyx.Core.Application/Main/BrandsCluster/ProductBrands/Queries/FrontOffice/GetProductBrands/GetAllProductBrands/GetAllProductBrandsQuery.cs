using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.FrontOffice.GetProductBrands.GetAllProductBrands;
public record GetAllProductBrandsQuery : IRequest<List<AllProductBrandDto>>;

public class GetAllProductBrandsQueryHandler : IRequestHandler<GetAllProductBrandsQuery, List<AllProductBrandDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductBrandsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductBrandDto>> Handle(GetAllProductBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.ProductBrands
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductBrandDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        foreach (var brand in brands)
        {
            var list = brand.Products?.Where(c => c.IsActive).ToList();
            brand.Products = list;
        }
        return brands;
    }
}
