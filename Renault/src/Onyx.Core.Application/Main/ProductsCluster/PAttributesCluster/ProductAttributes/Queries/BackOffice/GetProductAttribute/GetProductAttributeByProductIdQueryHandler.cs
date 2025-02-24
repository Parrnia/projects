using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.BackOffice.GetProductAttribute;


public record GetProductAttributeByProductIdQuery(int Id) : IRequest<List<ProductAttributeDto>>;


public class GetProductAttributeByProductIdQueryHandler : IRequestHandler<GetProductAttributeByProductIdQuery, List<ProductAttributeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductAttributeByProductIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductAttributeDto>> Handle(GetProductAttributeByProductIdQuery request, CancellationToken cancellationToken)
    {

        return await _context.ProductAttributes.AsNoTracking()
          .Where(x => x.ProductId == request.Id)
          .OrderBy(x => x.ProductId)
          .ProjectTo<ProductAttributeDto>(_mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken: cancellationToken);


    }
}
