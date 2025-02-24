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
public record GetProductsWithKindIdWithPaginationQuery : IRequest<PaginatedList<ProductDto>>
{
    public int KindId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
}
public class GetProductsWithKindIdWithPaginationQueryHandler : IRequestHandler<GetProductsWithKindIdWithPaginationQuery, PaginatedList<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsWithKindIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductDto>> Handle(GetProductsWithKindIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var products = _context.Products
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .Where(x => x.Kinds.Any(y => y.Id == request.KindId))
            .OrderBy(x => x.Name);

        ProductQueryHelperMethods.FilterProductsForRole(await products.ToListAsync(cancellationToken), request.CustomerTypeEnum);

        var result = await products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
        ProductQueryHelperMethods.FindSelectedAttributeOptionForProductDtos(result.Items, request.CustomerTypeEnum);

        return result;
    }
}
