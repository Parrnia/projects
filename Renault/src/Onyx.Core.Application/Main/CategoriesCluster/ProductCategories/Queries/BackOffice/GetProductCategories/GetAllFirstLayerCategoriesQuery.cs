using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.BackOffice.GetProductCategories;
public record GetAllFirstLayerCategoriesQuery : IRequest<List<ProductCategoryDto>>;

public class GetAllFirstLayerCategoriesQueryHandler : IRequestHandler<GetAllFirstLayerCategoriesQuery, List<ProductCategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllFirstLayerCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductCategoryDto>> Handle(GetAllFirstLayerCategoriesQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.ProductCategories
            .Where(c => c.ProductParentCategoryId == null)
            .OrderBy(x => x.Name)
            .ProjectTo<ProductCategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return brands;
    }
}
