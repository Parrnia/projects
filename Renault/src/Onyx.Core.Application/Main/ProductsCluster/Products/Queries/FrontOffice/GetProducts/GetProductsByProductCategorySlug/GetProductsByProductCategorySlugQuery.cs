using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetProductsByProductCategorySlug;

public record GetProductsByProductCategorySlugQuery : IRequest<List<ProductByProductCategorySlugDto>>
{
    public string? ProductCategorySlug { get; set; }
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
}
public class GetProductsByProductCategorySlugQueryHandler : IRequestHandler<GetProductsByProductCategorySlugQuery, List<ProductByProductCategorySlugDto>>
{
    private readonly IMapper _mapper;
    private readonly ISharedProductQueryHelperMethods _productQueryHelperMethods;
    private readonly IApplicationDbContext _context;


    public GetProductsByProductCategorySlugQueryHandler(IMapper mapper, ISharedProductQueryHelperMethods productQueryHelperMethods, IApplicationDbContext context)
    {
        _mapper = mapper;
        _productQueryHelperMethods = productQueryHelperMethods;
        _context = context;
    }

    public async Task<List<ProductByProductCategorySlugDto>> Handle(GetProductsByProductCategorySlugQuery request, CancellationToken cancellationToken)
    {
        if (request.ProductCategorySlug == null)
        {
            var productsWithoutCategory = _productQueryHelperMethods.GetProducts(c => c.IsActive);


            var helperResultWithoutCategory = await productsWithoutCategory.ProjectTo<ProductByProductCategorySlugModelHelperDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var resultWithoutCategory = _productQueryHelperMethods.MapNames(
                helperResultWithoutCategory,
                item => item.DisplayVariantName,
                item => item.Product,
                (product, name) => product.LocalizedName = name
            );

            resultWithoutCategory = _productQueryHelperMethods.FilterItemsForRole(
                resultWithoutCategory,
                product => product.AttributeOptions,
                option => option.ProductAttributeOptionRoles,
                role => role.CustomerTypeEnum == request.CustomerTypeEnum,
                (option, roles) => option.ProductAttributeOptionRoles = roles.ToList()
            );

            resultWithoutCategory = _productQueryHelperMethods.FindAndSetSelectedAttributeOption(
                resultWithoutCategory,
                product => product.AttributeOptions,
                option => option.ProductAttributeOptionRoles,
                role => role.CustomerTypeEnum == request.CustomerTypeEnum,
                role => role.Availability == AvailabilityEnum.InStock,
                option => option.IsDefault,
                (product, selectedOption) => product.SelectedProductAttributeOption = selectedOption
            );


            foreach (var product in resultWithoutCategory)
            {
                var list = product.Images.Where(c => c.IsActive).ToList();
                var list1 = product.Tags.Where(c => c.IsActive).ToList();
                product.Images = list;
                product.Tags = list1;
            }

            return resultWithoutCategory;
        }

        var lastChildrenIds = await GetLastChildrenBySlug(request.ProductCategorySlug, cancellationToken);

        var products = _productQueryHelperMethods.GetProducts(c => c.IsActive)
            .Where(w => lastChildrenIds.Contains(w.Product.ProductCategoryId));


        var helperResult = await products.ProjectTo<ProductByProductCategorySlugModelHelperDto>(_mapper.ConfigurationProvider)
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

    private async Task<List<int>> GetLastChildrenBySlug(string productCategorySlug, CancellationToken cancellationToken)
    {
        var productCategory = await _context.ProductCategories
            .Include(x => x.ProductChildrenCategories)!
            .ThenInclude(y => y.ProductChildrenCategories)!
            .ThenInclude(z => z.ProductChildrenCategories)!
            .ThenInclude(q => q.ProductChildrenCategories)
            .SingleOrDefaultAsync(x => x.Slug == productCategorySlug, cancellationToken: cancellationToken);
        if (productCategory != null)
        {
            return GetLastChildren(productCategory).Select(x => x.Id).ToList();
        }
        return new List<int>();
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
