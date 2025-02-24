using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.BackOffice.GetProductOptionMaterials;
public record GetAllProductOptionMaterialsDropDownQuery : IRequest<List<AllProductOptionMaterialDropDownDto>>;

public class GetAllProductOptionMaterialsDropDownQueryHandler : IRequestHandler<GetAllProductOptionMaterialsDropDownQuery, List<AllProductOptionMaterialDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductOptionMaterialsDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductOptionMaterialDropDownDto>> Handle(GetAllProductOptionMaterialsDropDownQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.ProductOptionMaterials.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductOptionMaterialDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
