using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.FrontOffice.GetProductTypeAttributeGroup;

public record GetProductTypeAttributeGroupByIdQuery(int Id) : IRequest<ProductTypeAttributeGroupByIdDto?>;

public class GetProductTypeAttributeGroupByIdQueryHandler : IRequestHandler<GetProductTypeAttributeGroupByIdQuery, ProductTypeAttributeGroupByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductTypeAttributeGroupByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductTypeAttributeGroupByIdDto?> Handle(GetProductTypeAttributeGroupByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductTypeAttributeGroups
            .ProjectTo<ProductTypeAttributeGroupByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
