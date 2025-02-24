using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.FrontOffice.GetProductTypeAttributeGroups.GetAllAttributeGroups;
public record GetAllProductTypeAttributeGroupsQuery : IRequest<List<AllProductTypeAttributeGroupDto>>;

public class GetAllProductTypeAttributeGroupsQueryHandler : IRequestHandler<GetAllProductTypeAttributeGroupsQuery, List<AllProductTypeAttributeGroupDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductTypeAttributeGroupsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductTypeAttributeGroupDto>> Handle(GetAllProductTypeAttributeGroupsQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.ProductTypeAttributeGroups.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductTypeAttributeGroupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return brands;
    }
}
