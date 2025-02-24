using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.BackOffice.GetProductOptionColors;
public record GetAllProductOptionColorsQuery : IRequest<List<ProductOptionColorDto>>;

public class GetAllProductOptionColorsQueryHandler : IRequestHandler<GetAllProductOptionColorsQuery, List<ProductOptionColorDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductOptionColorsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductOptionColorDto>> Handle(GetAllProductOptionColorsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.ProductOptionColors.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<ProductOptionColorDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
