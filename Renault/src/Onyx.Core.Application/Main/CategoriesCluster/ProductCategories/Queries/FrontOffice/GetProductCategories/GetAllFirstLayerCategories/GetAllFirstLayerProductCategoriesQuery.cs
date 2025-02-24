using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategories.GetAllFirstLayerCategories;
public record GetAllFirstLayerProductCategoriesQuery : IRequest<List<AllFirstLayerProductCategoryDto>>;

public class GetAllFirstLayerCategoriesQueryHandler : IRequestHandler<GetAllFirstLayerProductCategoriesQuery, List<AllFirstLayerProductCategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllFirstLayerCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllFirstLayerProductCategoryDto>> Handle(GetAllFirstLayerProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categoryDtos = await _context.ProductCategories
            .Where(c => c.ProductParentCategoryId == null && c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<AllFirstLayerProductCategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        return categoryDtos;
    }
}
