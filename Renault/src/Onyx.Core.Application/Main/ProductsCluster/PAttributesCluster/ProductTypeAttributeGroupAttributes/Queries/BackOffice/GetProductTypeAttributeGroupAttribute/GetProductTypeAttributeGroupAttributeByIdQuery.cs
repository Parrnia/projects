using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.BackOffice.GetProductTypeAttributeGroupAttribute;

public record GetProductTypeAttributeGroupAttributeByIdQuery(int Id) : IRequest<ProductTypeAttributeGroupAttributeDto?>;

public class GetProductTypeAttributeGroupAttributeByIdQueryHandler : IRequestHandler<GetProductTypeAttributeGroupAttributeByIdQuery, ProductTypeAttributeGroupAttributeDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductTypeAttributeGroupAttributeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductTypeAttributeGroupAttributeDto?> Handle(GetProductTypeAttributeGroupAttributeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductTypeAttributeGroupAttributes
            .ProjectTo<ProductTypeAttributeGroupAttributeDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
