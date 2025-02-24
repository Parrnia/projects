using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategories.GetAllProductCategories;
public record GetAllProductCategoriesQuery : IRequest<List<AllProductCategoryDto>>;

public class GetAllProductCategoriesQueryHandler : IRequestHandler<GetAllProductCategoriesQuery, List<AllProductCategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllProductCategoryDto>> Handle(GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categoryDtos = await _context.ProductCategories
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductCategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        foreach (var categoryDto in categoryDtos)
        {
            var list = categoryDto.ProductChildrenCategories?.Where(c => c.IsActive).ToList();
            categoryDto.ProductChildrenCategories = list;
        }

        return categoryDtos;
    }
}
