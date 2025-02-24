using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategoriesWithPagination.GetPopularFirstCategoriesWithPagination;
public record GetPopularFirstProductCategoriesWithPaginationQuery : IRequest<PaginatedList<PopularFirstProductCategoryWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetPopularFirstProductCategoriesWithPaginationQueryHandler : IRequestHandler<GetPopularFirstProductCategoriesWithPaginationQuery, PaginatedList<PopularFirstProductCategoryWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPopularFirstProductCategoriesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PopularFirstProductCategoryWithPaginationDto>> Handle(GetPopularFirstProductCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var categoryDtos = await _context.ProductCategories
            .Where(c => c.ProductParentCategoryId == null && c.IsPopular && c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<PopularFirstProductCategoryWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        foreach (var categoryDto in categoryDtos.Items)
        {
            var list = categoryDto.ProductChildrenCategories?.Where(c => c.IsActive).ToList();
            categoryDto.ProductChildrenCategories = list;
        }

        return categoryDtos;
    }
}
