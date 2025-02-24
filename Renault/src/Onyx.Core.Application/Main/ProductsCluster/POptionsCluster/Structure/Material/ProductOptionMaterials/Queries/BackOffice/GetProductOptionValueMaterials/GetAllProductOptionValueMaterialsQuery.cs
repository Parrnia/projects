using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.BackOffice.GetProductOptionValueMaterials;
public record GetAllProductOptionValueMaterialsQuery : IRequest<List<ProductOptionValueMaterialDto>>;

public class GetAllProductOptionValueMaterialsQueryHandler : IRequestHandler<GetAllProductOptionValueMaterialsQuery, List<ProductOptionValueMaterialDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductOptionValueMaterialsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductOptionValueMaterialDto>> Handle(GetAllProductOptionValueMaterialsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.ProductOptionValueMaterials
            .OrderBy(x => x.Name)
            .ProjectTo<ProductOptionValueMaterialDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
