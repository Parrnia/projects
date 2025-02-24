using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProduct.GetProductById;

public record GetProductByIdQuery : IRequest<ProductByIdDto?>
{
    public int Id { get; init; }
    public string ProductDisplayVariantName { get; init; } = null!;
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductByIdDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Where(c => c.IsActive)
            .ProjectTo<ProductByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        var productDisplayVariant = await _context.ProductDisplayVariants
            .SingleOrDefaultAsync(x => x.Name == request.ProductDisplayVariantName, cancellationToken);

        if (product != null)
        {
            ProductQueryHelperMethods.FilterProductByIdDtoForRole(product, request.CustomerTypeEnum);
            ProductQueryHelperMethods.FindSelectedAttributeOptionForProductByIdDto(product, request.CustomerTypeEnum);
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
