using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.BackOffice.GetProductOptionColors;
public record GetAllProductOptionColorsDropDownQuery : IRequest<List<AllProductOptionColorDropDownDto>>;

public class GetAllProductOptionColorsDropDownQueryHandler : IRequestHandler<GetAllProductOptionColorsDropDownQuery, List<AllProductOptionColorDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductOptionColorsDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductOptionColorDropDownDto>> Handle(GetAllProductOptionColorsDropDownQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.ProductOptionColors.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductOptionColorDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return posts;
    }
}
