using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.BackOffice.GetProductOptionMaterial;

public record GetProductOptionMaterialByIdQuery(int Id) : IRequest<ProductOptionMaterialDto?>;

public class GetProductOptionMaterialByIdQueryHandler : IRequestHandler<GetProductOptionMaterialByIdQuery, ProductOptionMaterialDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductOptionMaterialByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductOptionMaterialDto?> Handle(GetProductOptionMaterialByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductOptionMaterials
            .ProjectTo<ProductOptionMaterialDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
