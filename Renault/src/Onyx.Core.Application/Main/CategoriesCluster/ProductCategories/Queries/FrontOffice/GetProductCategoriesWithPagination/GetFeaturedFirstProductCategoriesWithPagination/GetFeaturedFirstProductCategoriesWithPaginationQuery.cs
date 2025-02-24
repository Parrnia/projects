using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategoriesWithPagination.GetFeaturedFirstProductCategoriesWithPagination;
public record GetFeaturedFirstProductCategoriesWithPaginationQuery : IRequest<PaginatedList<FeaturedFirstProductCategoryWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetFeaturedFirstProductCategoriesWithPaginationQueryHandler : IRequestHandler<GetFeaturedFirstProductCategoriesWithPaginationQuery, PaginatedList<FeaturedFirstProductCategoryWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFeaturedFirstProductCategoriesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<FeaturedFirstProductCategoryWithPaginationDto>> Handle(GetFeaturedFirstProductCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var categoryDtos = await _context.ProductCategories
            .Where(c => c.ProductParentCategoryId == null && c.IsFeatured && c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<FeaturedFirstProductCategoryWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        foreach (var categoryDto in categoryDtos.Items)
        {
            var list = categoryDto.ProductChildrenCategories?.Where(c => c.IsActive).ToList();
            categoryDto.ProductChildrenCategories = list;
        }

        return categoryDtos;
    }
}
