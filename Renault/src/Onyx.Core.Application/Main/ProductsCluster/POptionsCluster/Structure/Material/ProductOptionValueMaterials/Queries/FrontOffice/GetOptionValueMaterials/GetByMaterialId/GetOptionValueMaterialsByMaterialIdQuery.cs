using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionValueMaterials.Queries.FrontOffice.GetOptionValueMaterials.GetByMaterialId;
public record GetOptionValueMaterialsByMaterialIdQuery(int MaterialId) : IRequest<List<OptionValueMaterialByMaterialIdDto>>;

public class GetAllProductOptionValueMaterialsByMaterialIdQueryHandler : IRequestHandler<GetOptionValueMaterialsByMaterialIdQuery, List<OptionValueMaterialByMaterialIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductOptionValueMaterialsByMaterialIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OptionValueMaterialByMaterialIdDto>> Handle(GetOptionValueMaterialsByMaterialIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductOptionValueMaterials
            .Where(x => x.ProductOptionMaterialId == request.MaterialId)
            .OrderBy(x => x.ProductOptionMaterialId)
            .ProjectTo<OptionValueMaterialByMaterialIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}