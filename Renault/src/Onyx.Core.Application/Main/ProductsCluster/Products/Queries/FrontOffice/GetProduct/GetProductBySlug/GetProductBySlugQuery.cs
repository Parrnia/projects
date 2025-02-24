using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProduct.GetProductBySlug;

public record GetProductBySlugQuery : IRequest<ProductBySlugDto?>
{
    public string Slug { get; init; } = null!;
    public string ProductDisplayVariantName { get; init; } = null!;
    public CustomerTypeEnum CustomerTypeEnum { get; set; }

}

public class GetProductBySlugQueryHandler : IRequestHandler<GetProductBySlugQuery, ProductBySlugDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductBySlugQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductBySlugDto?> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Where(c => c.IsActive)
            .ProjectTo<ProductBySlugDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken: cancellationToken);
        var productDisplayVariant = await _context.ProductDisplayVariants
            .SingleOrDefaultAsync(x => x.Name == request.ProductDisplayVariantName, cancellationToken: cancellationToken);

        if (product != null)
        {
            ProductQueryHelperMethods.FilterProductBySlugDtoForRole(product, request.CustomerTypeEnum);
            ProductQueryHelperMethods.FindSelectedAttributeOptionForProductBySlugDto(product, request.CustomerTypeEnum);
            var list = product.Images.Where(c => c.IsActive).ToList();
            var list1 = product.Tags.Where(c => c.IsActive).ToList();
            product.Images = list;
            product.Tags = list1;
            if (productDisplayVariant != null)
            {
                product.LocalizedName = productDisplayVariant.Name;
            }
        }

        return product;
    }
}
