using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.BackOffice.GetProductTypeAttributeGroups;
public record GetAllProductTypeAttributeGroupsDropDownQuery : IRequest<List<AllProductTypeAttributeGroupDropDownDto>>;

public class GetAllProductTypeAttributeGroupsDropDownQueryHandler : IRequestHandler<GetAllProductTypeAttributeGroupsDropDownQuery, List<AllProductTypeAttributeGroupDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductTypeAttributeGroupsDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductTypeAttributeGroupDropDownDto>> Handle(GetAllProductTypeAttributeGroupsDropDownQuery request, CancellationToken cancellationToken)
    {
        var attributeGroupDropDownDtos = await _context.ProductTypeAttributeGroups.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductTypeAttributeGroupDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return attributeGroupDropDownDtos;
    }
}
