using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.FrontOffice.GetProductOptionColors;
public record GetAllProductOptionColorsQuery : IRequest<List<AllProductOptionColorDto>>;

public class GetAllProductOptionColorsQueryHandler : IRequestHandler<GetAllProductOptionColorsQuery, List<AllProductOptionColorDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductOptionColorsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductOptionColorDto>> Handle(GetAllProductOptionColorsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.ProductOptionColors.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductOptionColorDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
