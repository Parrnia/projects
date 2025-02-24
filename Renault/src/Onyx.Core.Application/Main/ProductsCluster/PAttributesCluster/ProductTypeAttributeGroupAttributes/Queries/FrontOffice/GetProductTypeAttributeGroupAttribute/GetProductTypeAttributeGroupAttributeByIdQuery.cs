using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.FrontOffice.GetProductTypeAttributeGroupAttribute;

public record GetProductTypeAttributeGroupAttributeByIdQuery(int Id) : IRequest<ProductTypeAttributeGroupAttributeByIdDto?>;

public class GetProductTypeAttributeGroupAttributeByIdQueryHandler : IRequestHandler<GetProductTypeAttributeGroupAttributeByIdQuery, ProductTypeAttributeGroupAttributeByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductTypeAttributeGroupAttributeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductTypeAttributeGroupAttributeByIdDto?> Handle(GetProductTypeAttributeGroupAttributeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductTypeAttributeGroupAttributes
            .ProjectTo<ProductTypeAttributeGroupAttributeByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
