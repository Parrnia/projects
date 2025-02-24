using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.SharedCluster.Search.Queries.FrontOffice.GetSearchSuggestions;

public record GetSearchSuggestionsQuery(int ProductLimit, int ProductCategoryLimit, CustomerTypeEnum CustomerTypeEnum, string? Query, int? KindId) : IRequest<SearchSuggestionDto>;

public class GetSearchSuggestionsQueryHandler : IRequestHandler<GetSearchSuggestionsQuery, SearchSuggestionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISharedProductQueryHelperMethods _productQueryHelperMethods;

    public GetSearchSuggestionsQueryHandler(IApplicationDbContext context, IMapper mapper, ISharedProductQueryHelperMethods productQueryHelperMethods)
    {
        _context = context;
        _mapper = mapper;
        _productQueryHelperMethods = productQueryHelperMethods;
    }

    public async Task<SearchSuggestionDto> Handle(GetSearchSuggestionsQuery request, CancellationToken cancellationToken)
    {
        var queryList = request.Query?.Split(' ').ToList();

        var productModelQuery = _productQueryHelperMethods.GetProducts(c => c.IsActive)
            .Where(e => e.Product.IsActive && request.KindId == -1 || e.Product.Kinds.Any(d =>
                d.Id == request.KindId || e.Product.Compatibility == CompatibilityEnum.All));

        var productCategoryQuery = _context.ProductCategories
            .OrderBy(x => x.LocalizedName)
            .ProjectTo<ProductCategoryForSearchSuggestionDto>(_mapper.ConfigurationProvider);

        if (queryList != null && request.Query != "NothingInserted" && queryList.Any())
        {
            foreach (var keyword in queryList.Select(c => c.ToLower()))
            {
                productModelQuery = productModelQuery
                    .Where(p => p.Product.LocalizedName.ToLower().Contains(keyword) || (p.DisplayVariantName != null && p.DisplayVariantName.ToLower().Contains(keyword.ToLower())));
            }

            foreach (var keyword in queryList)
            {
                productCategoryQuery = productCategoryQuery
                    .Where(pc => pc.LocalizedName.Contains(keyword) || pc.Name.Contains(keyword));
            }
        }

        var helperResult = await productModelQuery.ProjectTo<ProductForSearchSuggestionModelHelperDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(1, request.ProductLimit);

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

        var productDtos = new PaginatedList<ProductForSearchSuggestionDto>(result, helperResult.TotalCount, 1, request.ProductLimit);

        var productCategoryDtos = await productCategoryQuery.PaginatedListAsync(1, request.ProductCategoryLimit);

        var searchSuggestionDto = new SearchSuggestionDto()
        {
            Products = productDtos,
            ProductCategories = productCategoryDtos
        };

        return searchSuggestionDto;
    }

}
