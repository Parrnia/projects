using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetProductsByProductCategoryIdWithPagination;
public record GetProductsByProductCategoryIdWithPaginationQuery : IRequest<PaginatedList<ProductByProductCategoryIdWithPaginationDto>>
{
    public int ProductParentCategoryId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
}
public class GetProductsWithProductParentCategoryIdWithPaginationQueryHandler : IRequestHandler<GetProductsByProductCategoryIdWithPaginationQuery, PaginatedList<ProductByProductCategoryIdWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISharedProductQueryHelperMethods _productQueryHelperMethods;


    public GetProductsWithProductParentCategoryIdWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, ISharedProductQueryHelperMethods productQueryHelperMethods)
    {
        _context = context;
        _mapper = mapper;
        _productQueryHelperMethods = productQueryHelperMethods;
    }

    public async Task<PaginatedList<ProductByProductCategoryIdWithPaginationDto>> Handle(GetProductsByProductCategoryIdWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var productCategory = await _context.ProductCategories
            .Include(x => x.ProductChildrenCategories)!
            .ThenInclude(y => y.ProductChildrenCategories)!
            .ThenInclude(z => z.ProductChildrenCategories)!
            .ThenInclude(q => q.ProductChildrenCategories)
            .SingleOrDefaultAsync(x => x.Id == request.ProductParentCategoryId, cancellationToken: cancellationToken);

        var lastChildrenIds = new List<int>();
        if (productCategory != null)
        {
            lastChildrenIds = GetLastChildren(productCategory).Select(x => x.Id).ToList();
        }

        var products = _productQueryHelperMethods.GetProducts(c => c.IsActive)
            .Where(w => lastChildrenIds.Contains(w.Product.ProductCategoryId));


        var helperResult = await products.ProjectTo<ProductByProductCategoryIdWithPaginationModelHelperDto>(_mapper.ConfigurationProvider)
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

        var finalResult = new PaginatedList<ProductByProductCategoryIdWithPaginationDto>(result, helperResult.TotalCount, request.PageNumber, request.PageSize);
        return finalResult;
    }


    private List<ProductCategory> GetLastChildren(ProductCategory productCategory)
    {
        var result = new List<ProductCategory>();
        if (productCategory.ProductChildrenCategories?.Count != 0)
        {
            if (productCategory.ProductChildrenCategories != null)
            {
                foreach (var childProductCategory in productCategory.ProductChildrenCategories)
                {
                    result.AddRange(GetLastChildren(childProductCategory));
                }
            }
        }
        else
        {
            result.Add(productCategory);
        }
        return result;
    }

}
