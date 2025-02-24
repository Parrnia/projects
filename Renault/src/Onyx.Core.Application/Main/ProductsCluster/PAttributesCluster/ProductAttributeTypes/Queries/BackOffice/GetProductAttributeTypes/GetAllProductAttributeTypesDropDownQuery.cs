using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice.GetProductAttributeTypes;
public record GetAllProductAttributeTypesDropDownQuery : IRequest<List<AllProductAttributeTypeDropDownDto>>;

public class GetAllProductAttributeTypesDropDownQueryHandler : IRequestHandler<GetAllProductAttributeTypesDropDownQuery, List<AllProductAttributeTypeDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductAttributeTypesDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductAttributeTypeDropDownDto>> Handle(GetAllProductAttributeTypesDropDownQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.ProductAttributeTypes.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductAttributeTypeDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
