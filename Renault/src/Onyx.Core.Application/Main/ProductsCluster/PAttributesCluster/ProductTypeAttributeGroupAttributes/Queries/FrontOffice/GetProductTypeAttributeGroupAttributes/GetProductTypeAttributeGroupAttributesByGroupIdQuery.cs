using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.FrontOffice.GetProductTypeAttributeGroupAttributes;

public record GetProductTypeAttributeGroupAttributesByGroupIdQuery(int ProductTypeAttributeGroupId) : IRequest<List<ProductTypeAttributeGroupAttributeByGroupDto>>;

public class GetAllProductTypeAttributeGroupAttributeQueryHandler : IRequestHandler<GetProductTypeAttributeGroupAttributesByGroupIdQuery, List<ProductTypeAttributeGroupAttributeByGroupDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductTypeAttributeGroupAttributeQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductTypeAttributeGroupAttributeByGroupDto>> Handle(GetProductTypeAttributeGroupAttributesByGroupIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.ProductTypeAttributeGroupAttributes
            .Where(g => g.ProductTypeAttributeGroupId == request.ProductTypeAttributeGroupId)
            .OrderBy(x => x.Value)
            .ProjectTo<ProductTypeAttributeGroupAttributeByGroupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return res;
    }
}



