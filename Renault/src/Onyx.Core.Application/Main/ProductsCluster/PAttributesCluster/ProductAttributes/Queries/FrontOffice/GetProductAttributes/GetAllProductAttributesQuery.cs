using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.FrontOffice.GetProductAttributes;
public record GetAllProductAttributesQuery : IRequest<List<AllProductAttributeDto>>;

public class GetAllProductAttributesQueryHandler : IRequestHandler<GetAllProductAttributesQuery, List<AllProductAttributeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductAttributesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductAttributeDto>> Handle(GetAllProductAttributesQuery request, CancellationToken cancellationToken)
    {
        var productAttributes = await _context.ProductAttributes
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductAttributeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return productAttributes;
    }
}
