using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.HelperMethods;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.GetProduct;

public record GetProductByIdQuery : IRequest<ProductDto?>
{
    public int Id { get; init; }
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        if (product != null)
        {
            ProductQueryHelperMethods.FilterProductDtoForRole(product, request.CustomerTypeEnum);
            ProductQueryHelperMethods.FindSelectedAttributeOptionForProductDto(product, request.CustomerTypeEnum);
        }

        return product;
    }
}
