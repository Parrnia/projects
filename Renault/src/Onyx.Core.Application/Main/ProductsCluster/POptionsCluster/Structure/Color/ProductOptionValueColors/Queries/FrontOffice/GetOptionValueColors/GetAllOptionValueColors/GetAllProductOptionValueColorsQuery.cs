using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Queries.FrontOffice.GetOptionValueColors.GetAllOptionValueColors;
public record GetAllProductOptionValueColorsQuery : IRequest<List<AllProductOptionValueColorDto>>;

public class GetAllProductOptionValueColorsQueryHandler : IRequestHandler<GetAllProductOptionValueColorsQuery, List<AllProductOptionValueColorDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductOptionValueColorsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductOptionValueColorDto>> Handle(GetAllProductOptionValueColorsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.ProductOptionValueColors
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductOptionValueColorDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
