using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.BackOffice.GetProductTypeAttributeGroupAttributes;
public record GetAllProductTypeAttributeGroupAttributesQuery : IRequest<List<AllProductTypeAttributeGroupAttributeDto>>;

public class GetAllProductTypeAttributeGroupAttributesQueryHandler : IRequestHandler<GetAllProductTypeAttributeGroupAttributesQuery, List<AllProductTypeAttributeGroupAttributeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductTypeAttributeGroupAttributesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductTypeAttributeGroupAttributeDto>> Handle(GetAllProductTypeAttributeGroupAttributesQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.ProductTypeAttributeGroupAttributes.AsNoTracking()
            .OrderBy(x => x.Value)
            .ProjectTo<AllProductTypeAttributeGroupAttributeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}







