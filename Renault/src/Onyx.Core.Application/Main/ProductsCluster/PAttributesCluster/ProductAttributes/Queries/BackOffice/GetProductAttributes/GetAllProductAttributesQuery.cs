using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.BackOffice.GetProductAttributes;

public record GetAllProductAttributesQuery : IRequest<List<ProductAttributeDto>>;

public class GetAllProductAttributesQueryHandler : IRequestHandler<GetAllProductAttributesQuery, List<ProductAttributeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductAttributesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductAttributeDto>> Handle(GetAllProductAttributesQuery request, CancellationToken cancellationToken)
    {
        var productAttributes = await _context.ProductAttributes.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<ProductAttributeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return productAttributes;
    }
}
