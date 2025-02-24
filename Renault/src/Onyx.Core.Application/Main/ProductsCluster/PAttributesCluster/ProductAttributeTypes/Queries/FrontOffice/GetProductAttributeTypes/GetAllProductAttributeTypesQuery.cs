using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.FrontOffice.GetProductAttributeTypes;
public record GetAllProductAttributeTypesQuery : IRequest<List<AllProductAttributeTypeDto>>;

public class GetAllProductAttributeTypesQueryHandler : IRequestHandler<GetAllProductAttributeTypesQuery, List<AllProductAttributeTypeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductAttributeTypesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductAttributeTypeDto>> Handle(GetAllProductAttributeTypesQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.ProductAttributeTypes.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductAttributeTypeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
