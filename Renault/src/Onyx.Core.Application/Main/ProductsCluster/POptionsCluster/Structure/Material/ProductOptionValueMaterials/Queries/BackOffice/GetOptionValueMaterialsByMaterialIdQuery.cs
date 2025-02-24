using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.BackOffice;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionValueMaterials.Queries.BackOffice;
public record GetOptionValueMaterialsByMaterialIdQuery(int MaterialId) : IRequest<List<ProductOptionValueMaterialDto>>;

public class GetAllProductOptionValueMaterialsByMaterialIdQueryHandler : IRequestHandler<GetOptionValueMaterialsByMaterialIdQuery, List<ProductOptionValueMaterialDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductOptionValueMaterialsByMaterialIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductOptionValueMaterialDto>> Handle(GetOptionValueMaterialsByMaterialIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductOptionValueMaterials
            .Where(x => x.ProductOptionMaterialId == request.MaterialId)
            .OrderBy(x => x.ProductOptionMaterialId)
            .ProjectTo<ProductOptionValueMaterialDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}