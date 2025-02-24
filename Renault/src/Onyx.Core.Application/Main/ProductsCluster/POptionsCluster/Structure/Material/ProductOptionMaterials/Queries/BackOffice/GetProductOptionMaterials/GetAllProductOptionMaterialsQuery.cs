using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.BackOffice.GetProductOptionMaterials;
public record GetAllProductOptionMaterialsQuery : IRequest<List<ProductOptionMaterialDto>>;

public class GetAllProductOptionMaterialsQueryHandler : IRequestHandler<GetAllProductOptionMaterialsQuery, List<ProductOptionMaterialDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductOptionMaterialsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductOptionMaterialDto>> Handle(GetAllProductOptionMaterialsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.ProductOptionMaterials.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<ProductOptionMaterialDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
