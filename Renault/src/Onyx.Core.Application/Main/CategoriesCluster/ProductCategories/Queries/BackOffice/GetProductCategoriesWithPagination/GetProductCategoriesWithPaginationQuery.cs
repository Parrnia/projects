using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.BackOffice.GetProductCategoriesWithPagination;
public record GetProductCategoriesWithPaginationQuery : IRequest<PaginatedList<ProductCategoryDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProductCategoriesWithPaginationQueryHandler : IRequestHandler<GetProductCategoriesWithPaginationQuery, PaginatedList<ProductCategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductCategoriesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductCategoryDto>> Handle(GetProductCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductCategories
            .OrderBy(x => x.Name)
            .ProjectTo<ProductCategoryDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
