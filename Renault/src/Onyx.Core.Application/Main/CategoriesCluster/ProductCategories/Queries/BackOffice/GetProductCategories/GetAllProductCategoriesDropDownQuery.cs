using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.BackOffice.GetProductCategories;
public record GetAllProductCategoriesDropDownQuery : IRequest<List<AllProductCategoryDropDownDto>>;

public class GetAllProductCategoriesDropDownQueryHandler : IRequestHandler<GetAllProductCategoriesDropDownQuery, List<AllProductCategoryDropDownDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductCategoriesDropDownQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductCategoryDropDownDto>> Handle(GetAllProductCategoriesDropDownQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.ProductCategories
            .OrderBy(x => x.Code)
            .ProjectTo<AllProductCategoryDropDownDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return categories;
    }
}
