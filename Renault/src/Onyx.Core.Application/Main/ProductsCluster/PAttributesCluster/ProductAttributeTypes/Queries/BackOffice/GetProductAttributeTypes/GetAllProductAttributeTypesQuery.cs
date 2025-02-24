using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice.GetProductAttributeTypes;
public record GetAllProductAttributeTypesQuery : IRequest<List<ProductAttributeTypeDto>>;

public class GetAllProductAttributeTypesQueryHandler : IRequestHandler<GetAllProductAttributeTypesQuery, List<ProductAttributeTypeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductAttributeTypesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductAttributeTypeDto>> Handle(GetAllProductAttributeTypesQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.ProductAttributeTypes.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<ProductAttributeTypeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
