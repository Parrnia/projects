using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.BackOffice.GetProductTypeAttributeGroups;
public record GetAllProductTypeAttributeGroupsQuery : IRequest<List<ProductTypeAttributeGroupDto>>;

public class GetAllProductTypeAttributeGroupsQueryHandler : IRequestHandler<GetAllProductTypeAttributeGroupsQuery, List<ProductTypeAttributeGroupDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductTypeAttributeGroupsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductTypeAttributeGroupDto>> Handle(GetAllProductTypeAttributeGroupsQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.ProductTypeAttributeGroups.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<ProductTypeAttributeGroupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return brands;
    }
}
