using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.FrontOffice.GetProductOptionMaterial;

public record GetProductOptionMaterialByIdQuery(int Id) : IRequest<ProductOptionMaterialByIdDto?>;

public class GetProductOptionMaterialByIdQueryHandler : IRequestHandler<GetProductOptionMaterialByIdQuery, ProductOptionMaterialByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductOptionMaterialByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductOptionMaterialByIdDto?> Handle(GetProductOptionMaterialByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductOptionMaterials
            .ProjectTo<ProductOptionMaterialByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
