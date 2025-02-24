using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.BackOffice.GetProductTypeAttributeGroupAttributes;

public record GetAllProductTypeAttributeGroupAttributeByGroupQuery(int Id) : IRequest<List<ProductTypeAttributeGroupAttributeDto>>;

public class GetAllProductTypeAttributeGroupAttributeQueryHandler : IRequestHandler<GetAllProductTypeAttributeGroupAttributeByGroupQuery, List<ProductTypeAttributeGroupAttributeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductTypeAttributeGroupAttributeQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductTypeAttributeGroupAttributeDto>> Handle(GetAllProductTypeAttributeGroupAttributeByGroupQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.ProductTypeAttributeGroupAttributes.AsNoTracking().Where(g=>g.ProductTypeAttributeGroupId == request.Id)
            .OrderBy(x => x.Value)
            .ProjectTo<ProductTypeAttributeGroupAttributeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return res;
    }
}



