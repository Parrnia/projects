using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.FrontOffice.GetProductOptionMaterials;
public record GetAllProductOptionMaterialsQuery : IRequest<List<AllProductOptionMaterialDto>>;

public class GetAllProductOptionMaterialsQueryHandler : IRequestHandler<GetAllProductOptionMaterialsQuery, List<AllProductOptionMaterialDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductOptionMaterialsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductOptionMaterialDto>> Handle(GetAllProductOptionMaterialsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.ProductOptionMaterials.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductOptionMaterialDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
