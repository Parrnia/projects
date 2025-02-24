using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.HelperMethods;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.GetProducts;

public record GetProductsByProductCategoryIdQuery : IRequest<List<ProductDto>>
{
    public int ProductCategoryId { get; init; }
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
}
public class GetProductsByProductCategoryIdQueryHandler : IRequestHandler<GetProductsByProductCategoryIdQuery, List<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsByProductCategoryIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> Handle(GetProductsByProductCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var products = _context.Products
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .OrderBy(x => x.Name);

        ProductQueryHelperMethods.FilterProductsForRole(await products.ToListAsync(cancellationToken), request.CustomerTypeEnum);

        var result = await products
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        ProductQueryHelperMethods.FindSelectedAttributeOptionForProductDtos(result, request.CustomerTypeEnum);

        return result;
    }
}
