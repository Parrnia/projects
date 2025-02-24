using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.SharedCluster.Search.Queries.FrontOffice.GetSearchSuggestions;

public class SearchSuggestionDto
{
    public PaginatedList<ProductForSearchSuggestionDto> Products { get; set; } = null!;
    public PaginatedList<ProductCategoryForSearchSuggestionDto> ProductCategories { get; set; } = null!;
}

public class ProductForSearchSuggestionModelHelperDto : IMapFrom<ProductModelHelper>
{
    public ProductForSearchSuggestionDto Product { get; set; } = null!;
    public string? DisplayVariantName { get; set; }
}

public class ProductForSearchSuggestionDto : IMapFrom<Product>
{
    public ProductForSearchSuggestionDto()
    {
        Images = new List<ProductImageForSearchSuggestionDto>();
        AttributeOptions = new List<ProductAttributeOptionForSearchSuggestionDto>();
        KindIds = new List<int>();
    }
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public IList<ProductImageForSearchSuggestionDto> Images { get; set; }
    public int ReviewsLength { get; set; }
    public IList<int> KindIds { get; set; }
    public CompatibilityEnum Compatibility { get; set; }
    public int Rating { get; set; }
    public IList<ProductAttributeOptionForSearchSuggestionDto> AttributeOptions { get; set; }
    public ProductAttributeOptionForSearchSuggestionDto? SelectedProductAttributeOption { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductForSearchSuggestionDto>()
            .ForMember(d => d.KindIds, opt => opt.MapFrom(s => s.Kinds.Select(x => x.Id)))
            .ForMember(d => d.ReviewsLength, opt => opt.MapFrom(s => s.Reviews.Count == 0 ? 0 : s.Reviews.Count(c => c.IsActive)))
            .ForMember(d => d.Rating, opt => opt.MapFrom(s => s.Reviews.Count == 0 ? 0 : s.Reviews.Sum(c => c.Rating) / s.Reviews.Count));
    }

}
public class ProductImageForSearchSuggestionDto : IMapFrom<ProductImage>
{
    public int Id { get; set; }
    public Guid Image { get; set; }
    public int Order { get; set; }
}
public class ProductAttributeOptionForSearchSuggestionDto : IMapFrom<ProductAttributeOption>
{
    public int Id { get; set; }
    public List<ProductAttributeOptionValueForSearchSuggestionDto> OptionValues { get; set; } = new List<ProductAttributeOptionValueForSearchSuggestionDto>();
    public int ProductId { get; set; }
    public List<ProductAttributeOptionRoleForSearchSuggestionDto> ProductAttributeOptionRoles { get; set; } = new List<ProductAttributeOptionRoleForSearchSuggestionDto>();
    public bool IsDefault { get; set; }
    public List<PriceDto> Prices { get; set; } = new List<PriceDto>();
    public List<BadgeDto> Badges { get; set; } = new List<BadgeDto>();
}
public class ProductAttributeOptionValueForSearchSuggestionDto : IMapFrom<ProductAttributeOptionValue>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}
public class ProductAttributeOptionRoleForSearchSuggestionDto : IMapFrom<ProductAttributeOptionRole>
{
    public int Id { get; set; }
    public CustomerTypeEnum CustomerTypeEnum { get; set; }
    public double DiscountPercent { get; set; }
    public AvailabilityEnum Availability { get; set; }
    public double CurrentMaxOrderQty { get; set; }
    public double CurrentMinOrderQty { get; set; }
}
public class ProductCategoryForSearchSuggestionDto : IMapFrom<ProductCategory>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
}