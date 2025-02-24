using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Settings;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.SharedCluster.Search.Queries.FrontOffice.GetFilterResults;

public record GetFilterResultsQuery : IRequest<FilterViewModel>
{
    public CustomerTypeEnum CustomerType { get; set; }
    public string? ProductCategorySlug { get; set; }
    public int? KindId { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public List<string>? ProductBrandSlugs { get; set; }
    public List<string>? VehicleBrandSlugs { get; set; }
    public string? Discount { get; set; }
    public List<int>? Ratings { get; set; }
    public string? Color { get; set; }
    public string? Material { get; set; }
    public string? SearchText { get; set; } = null!;
    public int PageNumber { get; init; }
    public int Limit { get; init; }
    public string Sort { get; set; } = null!;
}

public class GetFilterResultsQueryHandler : IRequestHandler<GetFilterResultsQuery, FilterViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISharedProductQueryHelperMethods _productQueryHelperMethods;
    private readonly ApplicationSettings _applicationSettings;


    public GetFilterResultsQueryHandler(IApplicationDbContext context, IMapper mapper, ISharedProductQueryHelperMethods productQueryHelperMethods, IOptions<ApplicationSettings> applicationSettings)
    {
        _context = context;
        _mapper = mapper;
        _productQueryHelperMethods = productQueryHelperMethods;
        _applicationSettings = applicationSettings.Value;
    }

    public async Task<FilterViewModel> Handle(GetFilterResultsQuery request, CancellationToken cancellationToken)
    {
        var productModelQuery = _productQueryHelperMethods.GetProducts(c => c.IsActive);

        if (request.ProductCategorySlug != null)
        {
            var lastChildrenIds = await GetLastChildrenBySlug(request.ProductCategorySlug, cancellationToken);
            productModelQuery = productModelQuery.Where(w => lastChildrenIds.Contains(w.Product.ProductCategoryId));
        }

        if (request.KindId != null)
        {
            productModelQuery = productModelQuery.Where(x => x.Product.Compatibility == CompatibilityEnum.All || (x.Product.Compatibility != CompatibilityEnum.Unknown && x.Product.Kinds.Any(y => y.Id == request.KindId)));
        }

        if (request.MinPrice != null)
        {
            productModelQuery = productModelQuery.Where(x => x.Product.AttributeOptions
                .Any(c => c.Prices.OrderBy(c => c.Date).Last().MainPrice * (1 + ((decimal)_applicationSettings.TaxPercent / 100)) >= request.MinPrice * 10));
        }

        if (request.MaxPrice != null)
        {
            productModelQuery = productModelQuery.Where(x => x.Product.AttributeOptions
                .Any(c => c.Prices.OrderBy(c => c.Date).Last().MainPrice * (1 + ((decimal)_applicationSettings.TaxPercent / 100)) <= request.MaxPrice * 10));
        }

        if (request.ProductBrandSlugs != null)
        {
            productModelQuery = productModelQuery.Where(x => request.ProductBrandSlugs.Contains(x.Product.ProductBrand.Slug));
        }

        if (request.VehicleBrandSlugs != null)
        {
            productModelQuery = productModelQuery.Where(x => request.VehicleBrandSlugs.Any(e => x.Product.Kinds.Select(c => c.Model.Family.VehicleBrand.Slug).Contains(e)));
        }

        if (request.Ratings != null)
        {
            productModelQuery = productModelQuery.Where(x => request.Ratings.Any(r => x.Product.Reviews.Count == 0 || (x.Product.Reviews.Sum(e => e.Rating) / (x.Product.Reviews.Count())) == r));
        }

        if (request.Sort != "default")
        {
            switch (request.Sort)
            {
                case "name_asc":
                    productModelQuery = productModelQuery.OrderBy(c => c.Product.LocalizedName);
                    break;
                case "name_desc":
                    productModelQuery = productModelQuery.OrderByDescending(c => c.Product.LocalizedName);
                    break;
                case "rating_desc":
                    productModelQuery = productModelQuery.OrderBy(c => c.Product.Reviews.Sum(e => e.Rating) / c.Product.Reviews.Count());
                    break;
                case "rating_asc":
                    productModelQuery = productModelQuery.OrderByDescending(c => c.Product.Reviews.Sum(e => e.Rating) / c.Product.Reviews.Count());
                    break;
                case "price_desc":
                    productModelQuery = productModelQuery
                        .OrderBy(c => c.Product.AttributeOptions
                            .SelectMany(e => e.Prices)
                            .OrderByDescending(w => w.Date)
                            .Select(w => w.MainPrice)
                            .FirstOrDefault());
                    break;
                case "price_asc":
                    productModelQuery = productModelQuery
                        .OrderByDescending(c => c.Product.AttributeOptions
                            .SelectMany(e => e.Prices)
                            .OrderByDescending(w => w.Date)
                            .Select(w => w.MainPrice)
                            .FirstOrDefault());

                    break;
            }
        }

        if (request.Discount != null)
        {
            switch (request.Discount)
            {
                case "any":
                    break;
                case "yes":
                    productModelQuery = productModelQuery.Where(e => e.Product.AttributeOptions
                        .Any(t => t.ProductAttributeOptionRoles.SingleOrDefault(c => c.CustomerTypeEnum == request.CustomerType) != null &&
                                  t.Prices.Any() &&
                                  t.ProductAttributeOptionRoles.SingleOrDefault(f => f.CustomerTypeEnum == request.CustomerType)!.DiscountPercent != 0));
                    break;
                case "no":
                    productModelQuery = productModelQuery.Where(e => e.Product.AttributeOptions
                        .Any(t => t.ProductAttributeOptionRoles.SingleOrDefault(c => c.CustomerTypeEnum == request.CustomerType) != null &&
                                  t.Prices.Any() &&
                                  t.ProductAttributeOptionRoles.SingleOrDefault(f => f.CustomerTypeEnum == request.CustomerType)!.DiscountPercent == 0));
                    break;
            }
        }

        if (request.Color != null)
        {
            var colors = request.Color.Split(',').ToList();
            productModelQuery = productModelQuery
                .Where(e => e.Product.AttributeOptions
                    .Any(t =>
                              (t.OptionValues.Any(q => q.Name.ToLower() == "color" && colors.Contains(q.Value))
                              || t.OptionValues.All(q => q.Name.ToLower() != "color"))));
        }

        if (request.Material != null)
        {
            var materials = request.Material.Split(',').ToList();
            productModelQuery = productModelQuery
                .Where(e => e.Product.AttributeOptions
                    .Any(t =>
                              (t.OptionValues.Any(q => q.Name.ToLower() == "material" && materials.Contains(q.Value))
                               || t.OptionValues.All(q => q.Name.ToLower() != "material"))));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            var queryList = request.SearchText.Split(' ').ToList();
            if (queryList.Any())
            {
                foreach (var keyword in queryList.Select(c => c.ToLower()))
                {
                    productModelQuery = productModelQuery
                        .Where(p => p.Product.LocalizedName.ToLower().Contains(keyword) || p.Product.Name.ToLower().Contains(keyword) || (p.Product.ProductNo != null && p.Product.ProductNo.ToLower().Contains(keyword) || (p.DisplayVariantName != null && p.DisplayVariantName.ToLower().Contains(keyword.ToLower()))));
                }
            }
        }


        var helperResult = await productModelQuery.ProjectTo<ProductForFilterResultModelHelperDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.Limit);

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
            role => role.CustomerTypeEnum == request.CustomerType,
            (option, roles) => option.ProductAttributeOptionRoles = roles.ToList()
        );

        result = _productQueryHelperMethods.FindAndSetSelectedAttributeOption(
            result,
            product => product.AttributeOptions,
            option => option.ProductAttributeOptionRoles,
            role => role.CustomerTypeEnum == request.CustomerType,
            role => role.Availability == AvailabilityEnum.InStock,
            option => option.IsDefault,
            (product, selectedOption) => product.SelectedProductAttributeOption = selectedOption
        );

        
        foreach (var productDto in result)
        {
            productDto.AttributeOptions = productDto.AttributeOptions.Where(c => c.ProductAttributeOptionRoles.Any(e => e.CustomerTypeEnum == request.CustomerType)).ToList();
            foreach (var attributeOptionDto in productDto.AttributeOptions)
            {
                attributeOptionDto.ProductAttributeOptionRoles = attributeOptionDto.ProductAttributeOptionRoles
                    .Where(c => c.CustomerTypeEnum == request.CustomerType).ToList();
            }
        }

        var totalCount = await productModelQuery.CountAsync(cancellationToken);

        if (request.Color != null || request.Material != null || request.Discount != null)
        {
            var colors = request.Color?.Split(',').ToList();
            var materials = request.Material?.Split(',').ToList();

            var matchingProducts = new List<MatchingProductModel>();
            foreach (var productModel in productModelQuery)
            {
                foreach (var option in productModel.Product.AttributeOptions)
                {

                    var productAttributeOptionRole = option.ProductAttributeOptionRoles.SingleOrDefault(c => c.CustomerTypeEnum == request.CustomerType);
                    var colorFilter = (colors == null ||
                                       (option.OptionValues.Any(value =>
                                           value.Name.ToLower() == "color" && colors.Contains(value.Value))) ||
                                       option.OptionValues.All(q => q.Name.ToLower() != "color"));
                    var materialFilter = materials == null ||
                                         (option.OptionValues.Any(value =>
                                              value.Name.ToLower() == "material" && materials.Contains(value.Value)) ||
                                          option.OptionValues.All(q => q.Name.ToLower() != "material"));
                    var discountFilter = request.Discount == null ||
                                         request.Discount == "any" ||
                                         (request.Discount == "yes" && productAttributeOptionRole?.DiscountPercent != 0) ||
                                         (request.Discount == "no" && productAttributeOptionRole?.DiscountPercent == 0);
                    if (colorFilter && materialFilter && discountFilter)
                    {
                        matchingProducts.Add(new MatchingProductModel
                        {
                            ProductId = productModel.Product.Id,
                            AttributeOptionId = option.Id
                        });
                    }


                }
            }

            foreach (var product in result)
            {
                foreach (var matchingProduct in matchingProducts.ToList())
                {
                    if (matchingProduct.ProductId == product.Id)
                    {
                        foreach (var productAttributeOption in product.AttributeOptions)
                        {
                            productAttributeOption.IsDefault = productAttributeOption.Id == matchingProduct.AttributeOptionId;
                        }
                    }
                }
            }

        }



        return new FilterViewModel()
        {
            Products = result,
            ProductsCount = totalCount
        };
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

public class FilterViewModel
{
    public FilterViewModel()
    {
        Products = new List<ProductForFilterResultDto>();
    }
    public List<ProductForFilterResultDto> Products { get; set; }
    public int ProductsCount { get; set; }
}
public class MatchingProductModel
{
    public int ProductId { get; set; }
    public int AttributeOptionId { get; set; }
}
