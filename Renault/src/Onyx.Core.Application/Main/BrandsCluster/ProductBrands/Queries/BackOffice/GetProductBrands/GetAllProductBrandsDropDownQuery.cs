using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.BackOffice.GetProductBrands;
public record GetAllProductBrandsDropDownQuery : IRequest<List<AllProductBrandDropDownDto>>;

public class GetAllProductBrandsDropDownQueryHandler : IRequestHandler<GetAllProductBrandsDropDownQuery, List<AllProductBrandDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductBrandsDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductBrandDropDownDto>> Handle(GetAllProductBrandsDropDownQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.ProductBrands
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductBrandDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return brands;
    }
}
