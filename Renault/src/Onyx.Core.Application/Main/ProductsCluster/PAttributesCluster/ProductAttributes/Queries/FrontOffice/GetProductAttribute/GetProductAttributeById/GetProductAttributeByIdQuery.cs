using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.FrontOffice.GetProductAttribute.GetProductAttributeById;

public record GetProductAttributeByIdQuery(int Id) : IRequest<ProductAttributeByIdDto?>;

public class GetProductAttributeByIdQueryHandler : IRequestHandler<GetProductAttributeByIdQuery, ProductAttributeByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductAttributeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductAttributeByIdDto?> Handle(GetProductAttributeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductAttributes
            .ProjectTo<ProductAttributeByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}

