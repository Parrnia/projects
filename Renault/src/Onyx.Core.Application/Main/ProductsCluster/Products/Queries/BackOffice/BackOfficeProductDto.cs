using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice;

public class BackOfficeProductDto : IMapFrom<Product>
{

    public int Id { get; set; }
    public int Code { get; set; }
    public string? ProductNo { get; set; }
    public string? OldProductNo { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? ProductCatalog { get; set; }
    public double OrderRate { get; set; }
    public int? Mileage { get; set; }
    public int? Duration { get; set; }
    public string Excerpt { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Sku { get; set; }



    public int? ProviderId { get; set; }
    public string? ProviderName { get; set; }
    public int? CountryId { get; set; }
    public string? CountryName { get; set; }
    public int? ProductTypeId { get; set; }
    public string? ProductTypeName { get; set; }
    public int? ProductStatusId { get; set; }
    public string? ProductStatusName { get; set; }
    public int? MainCountingUnitId { get; set; }
    public string? MainCountingUnitName { get; set; }
    public int? CommonCountingUnitId { get; set; }
    public string? CommonCountingUnitName { get; set; }
    public int BrandId { get; set; }
    public string BrandName { get; set; } = null!;
    public int ProductCategoryId { get; set; }
    public string ProductCategoryName { get; set; } = null!;
    public int? ProductAttributeTypeId { get; set; }
    public string ProductAttributeTypeName { get; set; } = null!;
    public int? ColorOptionId { get; set; }
    public string? ColorOptionName { get; set; }
    public int? MaterialOptionId { get; set; }
    public string? MaterialOptionName { get; set; }
    public bool IsActive { get; set; }

    //public ProductStockEnum Stock { get; set; }
    //public string StockName => EnumHelper<ProductStockEnum>.GetDisplayValue(Stock);

    //public AvailabilityEnum Availability { get; set; }
    //public string AvailabilityName => EnumHelper<AvailabilityEnum>.GetDisplayValue(Availability);

    public CompatibilityEnum Compatibility { get; set; }
    public string CompatibilityName => EnumHelper<CompatibilityEnum>.GetDisplayValue(Compatibility);


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, BackOfficeProductDto>()
            .ForMember(d => d.ProviderName, opt => opt.MapFrom(s => s.Provider != null ? s.Provider.LocalizedName : null))
            .ForMember(d => d.ProviderId, opt => opt.MapFrom(s => s.ProviderId))
            .ForMember(d => d.CountryName, opt => opt.MapFrom(s => s.Country != null ? s.Country.LocalizedName : null))
            .ForMember(d => d.CountryId, opt => opt.MapFrom(s => s.CountryId))
            .ForMember(d => d.ProductTypeId, opt => opt.MapFrom(s => s.ProductTypeId))
            .ForMember(d => d.ProductTypeName, opt => opt.MapFrom(s => s.ProductType != null ? s.ProductType.LocalizedName : null))
            .ForMember(d => d.ProductStatusName, opt => opt.MapFrom(s => s.ProductStatus != null ? s.ProductStatus.LocalizedName : null))
            .ForMember(d => d.ProductStatusId, opt => opt.MapFrom(s => s.ProductStatusId))
            .ForMember(d => d.ProductAttributeTypeId, opt => opt.MapFrom(s => s.ProductAttributeType.Id))
            .ForMember(d => d.ProductAttributeTypeName, opt => opt.MapFrom(s => s.ProductAttributeType.Name))
            .ForMember(d => d.MainCountingUnitId, opt => opt.MapFrom(s => s.MainCountingUnitId))
            .ForMember(d => d.MainCountingUnitName, opt => opt.MapFrom(s => s.MainCountingUnit != null ? s.MainCountingUnit.LocalizedName : null))
            .ForMember(d => d.CommonCountingUnitId, opt => opt.MapFrom(s => s.CommonCountingUnitId))
            .ForMember(d => d.CommonCountingUnitName, opt => opt.MapFrom(s => s.CommonCountingUnit != null ? s.CommonCountingUnit.LocalizedName : null))
            .ForMember(d => d.BrandId, opt => opt.MapFrom(s => s.ProductBrand.Id))
            .ForMember(d => d.BrandName, opt => opt.MapFrom(s => s.ProductBrand.LocalizedName))
            .ForMember(d => d.ProductCategoryId, opt => opt.MapFrom(s => s.ProductCategory.Id))
            .ForMember(d => d.ProductCategoryName, opt => opt.MapFrom(s => s.ProductCategory.LocalizedName))
            .ForMember(d => d.ColorOptionId, opt => opt.MapFrom(s => s.ColorOptionId))
            .ForMember(d => d.ColorOptionName, opt => opt.MapFrom(s => s.ColorOption != null ? s.ColorOption.Name : null))
            .ForMember(d => d.MaterialOptionId, opt => opt.MapFrom(s => s.MaterialOptionId))
            .ForMember(d => d.MaterialOptionName, opt => opt.MapFrom(s => s.MaterialOption != null ? s.MaterialOption.Name : null));
    }
}

