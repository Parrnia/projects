using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.BackOffice;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Queries.BackOffice;
public record GetAllProductOptionValueColorsQuery : IRequest<List<ProductOptionValueColorDto>>;

public class GetAllProductOptionValueColorsQueryHandler : IRequestHandler<GetAllProductOptionValueColorsQuery, List<ProductOptionValueColorDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductOptionValueColorsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductOptionValueColorDto>> Handle(GetAllProductOptionValueColorsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.ProductOptionValueColors
            .OrderBy(x => x.Name)
            .ProjectTo<ProductOptionValueColorDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
