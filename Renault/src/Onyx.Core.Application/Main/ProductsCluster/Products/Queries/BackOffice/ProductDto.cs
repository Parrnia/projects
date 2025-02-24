using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Main.InfoCluster.Countries.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.Providers.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.Tags.Queries.BackOffice;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice;
public class ProductDto : IMapFrom<Product>
{
    public ProductDto()
    {
        Images = new List<ProductImageDto>();
        Tags = new List<TagDto>();
        Attributes = new List<ProductAttributeDto>();
        AttributeOptions = new List<ProductAttributeOptionForProductDto>();
        ProductCustomFields = new List<ProductCustomFieldDto>();
        Reviews = new List<ReviewForProductDto>();
        KindIds = new List<int>();
    }
    public int Id { get; set; }
    public int Code { get; set; }
    public string? ProductNo { get; set; }
    public string? OldProductNo { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? ProductCatalog { get; set; }
    public double OrderRate { get; set; }
    public decimal? Height { get; set; }
    public decimal? Width { get; set; }
    public decimal? Length { get; set; }
    public decimal? NetWeight { get; set; }
    public decimal? GrossWeight { get; set; }
    public decimal? VolumeWeight { get; set; }
    public int? Mileage { get; set; }
    public int? Duration { get; set; }
    public bool DirectSalesLicense { get; set; }


    public ProviderDto? Provider { get; set; }
    public CountryDto? Country { get; set; }
    public ProductTypeDto? ProductType { get; set; }
    public ProductStatusDto? ProductStatus { get; set; }


    public string Excerpt { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Sku { get; set; }
    public IList<ProductImageDto> Images { get; set; }
    public CompatibilityEnum Compatibility { get; set; }
    public ProductAttributeTypeDto ProductAttributeType { get; set; } = null!;
    public IList<TagDto> Tags { get; set; }
    public IList<ProductAttributeDto> Attributes { get; set; }
    public IList<ProductAttributeOptionForProductDto> AttributeOptions { get; set; }
    public ProductOptionColorDto? ColorOption { get; set; }
    public ProductOptionMaterialDto? MaterialOption { get; set; }
    public IList<ProductCustomFieldDto> ProductCustomFields { get; set; }
    public IList<ReviewForProductDto> Reviews { get; set; }


    public BrandForProductDto ProductBrand { get; set; } = null!;
    public ProductCategoryForProductDto ProductCategory { get; set; } = null!;
    public IList<int> KindIds { get; set; }
    public ProductAttributeOptionForProductDto? SelectedProductAttributeOption { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductDto>()
            .ForMember(d => d.KindIds, opt => opt.MapFrom(s => s.Kinds.Select(x => x.Id)));
    }
}

