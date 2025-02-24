using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.HelperMethods;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.GetProductsWithPagination;
public record GetProductsWithProductCategoryIdWithPaginationQuery : IRequest<PaginatedList<ProductDto>>
{
    public int ProductCategoryId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
}
public class GetProductsWithProductCategoryIdWithPaginationQueryHandler : IRequestHandler<GetProductsWithProductCategoryIdWithPaginationQuery, PaginatedList<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsWithProductCategoryIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductDto>> Handle(GetProductsWithProductCategoryIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var products = _context.Products
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .Where(x => x.ProductCategoryId == request.ProductCategoryId)
            .OrderBy(x => x.Name);

        ProductQueryHelperMethods.FilterProductsForRole(await products.ToListAsync(cancellationToken), request.CustomerTypeEnum);

        var result = await products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        ProductQueryHelperMethods.FindSelectedAttributeOptionForProductDtos(result.Items, request.CustomerTypeEnum);

        return result;
    }
}
