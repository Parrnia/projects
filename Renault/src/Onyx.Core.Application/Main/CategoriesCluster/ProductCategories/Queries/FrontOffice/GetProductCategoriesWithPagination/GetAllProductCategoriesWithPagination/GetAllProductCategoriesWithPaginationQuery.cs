using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategoriesWithPagination.GetAllProductCategoriesWithPagination;
public record GetAllProductCategoriesWithPaginationQuery : IRequest<PaginatedList<AllProductCategoryWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetAllProductCategoriesWithPaginationQueryHandler : IRequestHandler<GetAllProductCategoriesWithPaginationQuery, PaginatedList<AllProductCategoryWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductCategoriesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AllProductCategoryWithPaginationDto>> Handle(GetAllProductCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var categoryDtos = await _context.ProductCategories
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<AllProductCategoryWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        foreach (var categoryDto in categoryDtos.Items)
        {
            var list = categoryDto.ProductChildrenCategories?.Where(c => c.IsActive).ToList();
            categoryDto.ProductChildrenCategories = list;
        }

        return categoryDtos;
    }
}
