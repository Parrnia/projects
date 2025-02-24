using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.BackOffice.GetProductTypeAttributeGroup;

public record GetProductTypeAttributeGroupByIdQuery(int Id) : IRequest<ProductTypeAttributeGroupDto?>;

public class GetProductTypeAttributeGroupByIdQueryHandler : IRequestHandler<GetProductTypeAttributeGroupByIdQuery, ProductTypeAttributeGroupDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductTypeAttributeGroupByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductTypeAttributeGroupDto?> Handle(GetProductTypeAttributeGroupByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductTypeAttributeGroups
            .ProjectTo<ProductTypeAttributeGroupDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
