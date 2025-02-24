using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetProductsByBrandIdWithPagination;
public record GetProductsByBrandIdWithPaginationQuery : IRequest<PaginatedList<ProductByBrandIdWithPaginationDto>>
{
    public int BrandId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
}
public class GetProductsWithBrandIdWithPaginationQueryHandler : IRequestHandler<GetProductsByBrandIdWithPaginationQuery, PaginatedList<ProductByBrandIdWithPaginationDto>>
{
    private readonly ISharedProductQueryHelperMethods _productQueryHelperMethods;
    private readonly IMapper _mapper;

    public GetProductsWithBrandIdWithPaginationQueryHandler(IMapper mapper, ISharedProductQueryHelperMethods productQueryHelperMethods)
    {
        _mapper = mapper;
        _productQueryHelperMethods = productQueryHelperMethods;
    }

    public async Task<PaginatedList<ProductByBrandIdWithPaginationDto>> Handle(GetProductsByBrandIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var products = _productQueryHelperMethods.GetProducts(c => c.IsActive)
            .Where(x => x.Product.ProductBrandId == request.BrandId);

        var helperResult = await products.ProjectTo<ProductByBrandIdWithPaginationModelHelperDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        var result = _productQueryHelperMethods.MapNames(
            helperResult.Items,
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

        var finalResult = new PaginatedList<ProductByBrandIdWithPaginationDto>(result, helperResult.TotalCount, request.PageNumber, request.PageSize);
        return finalResult;
    }
}
