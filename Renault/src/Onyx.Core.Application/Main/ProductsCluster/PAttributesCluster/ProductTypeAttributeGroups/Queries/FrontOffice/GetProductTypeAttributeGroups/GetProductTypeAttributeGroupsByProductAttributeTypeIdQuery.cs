using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.FrontOffice.GetProductTypeAttributeGroups;

public record GetProductTypeAttributeGroupsByProductAttributeTypeIdQuery(int ProductAttributeTypeId) : IRequest<List<ProductTypeAttributeGroupByProductAttributeTypeIdDto>>;

public class GetProductTypeAttributeGroupsByProductAttributeTypeIdQueryHandler : IRequestHandler<GetProductTypeAttributeGroupsByProductAttributeTypeIdQuery, List<ProductTypeAttributeGroupByProductAttributeTypeIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductTypeAttributeGroupsByProductAttributeTypeIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductTypeAttributeGroupByProductAttributeTypeIdDto>> Handle(GetProductTypeAttributeGroupsByProductAttributeTypeIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductTypeAttributeGroups.AsNoTracking()
            .Where(x => x.ProductAttributeTypes.Select(p => p.Id).Contains(request.ProductAttributeTypeId))
            .OrderBy(x => x.Name)
            .ProjectTo<ProductTypeAttributeGroupByProductAttributeTypeIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
