using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.FrontOffice.GetProductAttribute.ProductAttributeByProductId;


public record GetProductAttributeByProductIdQuery(int ProductId) : IRequest<List<ProductAttributeByProductIdDto>>;


public class GetProductAttributeByProductIdQueryHandler : IRequestHandler<GetProductAttributeByProductIdQuery, List<ProductAttributeByProductIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductAttributeByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductAttributeByProductIdDto>> Handle(GetProductAttributeByProductIdQuery request, CancellationToken cancellationToken)
    {

        return await _context.ProductAttributes.AsNoTracking()
          .Where(x => x.ProductId == request.ProductId)
          .OrderBy(x => x.ProductId)
          .ProjectTo<ProductAttributeByProductIdDto>(_mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken: cancellationToken);


    }
}
