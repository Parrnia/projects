using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetProductsByBrandId;

public record GetProductsByBrandIdQuery : IRequest<List<ProductByBrandIdDto>>
{
    public int BrandId { get; init; }
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
}

public class GetProductsByBrandIdQueryHandler : IRequestHandler<GetProductsByBrandIdQuery, List<ProductByBrandIdDto>>
{
    private readonly IMapper _mapper;
    private readonly ISharedProductQueryHelperMethods _productQueryHelperMethods;


    public GetProductsByBrandIdQueryHandler(IMapper mapper, ISharedProductQueryHelperMethods productQueryHelperMethods)
    {
        _mapper = mapper;
        _productQueryHelperMethods = productQueryHelperMethods;
    }

    public async Task<List<ProductByBrandIdDto>> Handle(GetProductsByBrandIdQuery request, CancellationToken cancellationToken)
    {
        var products = _productQueryHelperMethods.GetProducts(c => c.IsActive)
            .Where(p => p.Product.ProductBrandId == request.BrandId);

        var helperResult = await products.ProjectTo<ProductByBrandIdModelHelperDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var result = _productQueryHelperMethods.MapNames(
            helperResult,
            item => item.DisplayVariantName,
            item => item.Product,
            (product, name) => product.LocalizedName = name
        );

        result = _productQueryHelperMethods.FilterItemsForRole(
            result,
            product => product.AttributeOptions,
            option => option.ProductAttributeOptionRoles,
            role => role.CustomerTypeEnum == request.CustomerTypeEnum,
            (option, roles) => option.ProductAttributeOptionRoles = roles.ToList()
        );

        result = _productQueryHelperMethods.FindAndSetSelectedAttributeOption(
            result,
            product => product.AttributeOptions,
            option => option.ProductAttributeOptionRoles,
            role => role.CustomerTypeEnum == request.CustomerTypeEnum,
            role => role.Availability == AvailabilityEnum.InStock,
            option => option.IsDefault,
            (product, selectedOption) => product.SelectedProductAttributeOption = selectedOption
        );


        foreach (var product in result)
        {
            var list = product.Images.Where(c => c.IsActive).ToList();
            var list1 = product.Tags.Where(c => c.IsActive).ToList();
            product.Images = list;
            product.Tags = list1;
        }

        return result;
    }
}
