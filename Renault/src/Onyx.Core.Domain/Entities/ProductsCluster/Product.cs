using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.CategoriesCluster;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Color;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Structure.Material;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;


namespace Onyx.Domain.Entities.ProductsCluster;
public class Product : BaseAuditableEntity
{
    /// <summary>
    /// کلید اصلی در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftProductId { get; set; }
    /// <summary>
    /// کد شمارنده
    /// </summary>
    public int Code { get; set; }
    /// <summary>
    /// کد کالا
    /// </summary>
    public string? ProductNo { get; set; }
    /// <summary>
    /// کد کالا قبلی
    /// </summary>
    public string? OldProductNo { get; set; }
    /// <summary>
    /// نام فارسی قطعه
    /// </summary>
    public string LocalizedName { get; set; } = null!;
    /// <summary>
    /// نام لاتین قطعه
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// کد شناسایی کالا در کاتالوگ
    /// </summary>
    public string? ProductCatalog { get; set; }
    /// <summary>
    /// ضریب سفارش دهی
    /// </summary>
    public double OrderRate { get; set; }
    /// <summary>
    /// ارتفاع کالا
    /// </summary>
    public decimal? Height { get; set; }
    /// <summary>
    /// عرض کالا
    /// </summary>
    public decimal? Width { get; set; }
    /// <summary>
    /// طول کالا
    /// </summary>
    public decimal? Length { get; set; }
    /// <summary>
    /// وزن خالص کالا
    /// </summary>
    public decimal? NetWeight { get; set; }
    /// <summary>
    /// وزن ناخالص کالا
    /// </summary>
    public decimal? GrossWeight { get; set; }
    /// <summary>
    /// وزن حجمی کالا
    /// </summary>
    public decimal? VolumeWeight { get; set; }
    /// <summary>
    /// کیلومتر گارانتی
    /// </summary>
    public int? Mileage { get; set; }
    /// <summary>
    /// تعداد ماه گارانتی
    /// </summary>
    public int? Duration { get; set; }
    /// <summary>
    /// کد تامین کننده
    /// </summary>
    public Provider? Provider { get; set; }
    public int? ProviderId { get; set; }
    /// <summary>
    /// کد کشور
    /// </summary>
    public Country? Country { get; set; }
    public int? CountryId { get; set; }
    /// <summary>
    /// کد نوع کالا
    /// </summary>
    public ProductType? ProductType { get; set; }
    public int? ProductTypeId { get; set; }
    /// <summary>
    /// کد وضعیت کالا
    /// </summary>
    public ProductStatus? ProductStatus { get; set; }
    public int? ProductStatusId { get; set; }
    /// <summary>
    /// واحد شمارش اصلی
    /// </summary>
    public CountingUnit? MainCountingUnit { get; set; }
    public int? MainCountingUnitId { get; set; }
    /// <summary>
    /// واحد شمارش رایج
    /// </summary>
    public CountingUnit? CommonCountingUnit { get; set; }
    public int? CommonCountingUnitId { get; set; }
    /// <summary>
    /// برند
    /// </summary>
    public ProductBrand ProductBrand { get; set; } = null!;
    public int ProductBrandId { get; set; }
    /// <summary>
    /// زیردسته کالا
    /// </summary>
    public ProductCategory ProductCategory { get; set; } = null!;
    public int ProductCategoryId { get; set; }
    /// <summary>
    /// نوع های مرتبط
    /// </summary>
    public List<Kind> Kinds { get; set; } = new List<Kind>();
    /// <summary>
    /// گزیده
    /// </summary>
    public string Excerpt { get; set; } = null!;
    /// <summary>
    /// توضیحات
    /// </summary>
    public string Description { get; set; } = null!;
    /// <summary>
    /// عنوان کوتاه
    /// </summary>
    public string Slug { get; set; } = null!;
    /// <summary>
    /// واحد نگهداری موجودی
    /// </summary>
    public string? Sku { get; set; } //یک بارکد قابل اسکن است که برای ایجاد تمایز بین محصولات مختلف به صورت لیبل پشت چسبدار روی محصول الصاق می‌شود.
    /// <summary>
    /// تصاویر محصول
    /// </summary>
    public List<ProductImage> Images { get; set; } = new List<ProductImage>();
    /// <summary>
    /// سازگاری محصول
    /// </summary>
    public CompatibilityEnum Compatibility { get; set; }
    /// <summary>
    /// نوع ویژگی محصول
    /// </summary>
    public ProductAttributeType ProductAttributeType { get; private set; } = null!;
    public int ProductAttributeTypeId { get; private set; }
    /// <summary>
    /// تگ ها
    /// </summary>
    public List<Tag> Tags { get; set; } = new List<Tag>();
    /// <summary>
    /// ویژگی های محصول
    /// </summary>
    public List<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
    /// <summary>
    /// ویژگی های گزینه های محصول
    /// </summary>
    public List<ProductAttributeOption> AttributeOptions { get; set; } = new List<ProductAttributeOption>();
    /// <summary>
    /// گزینه رنگ محصول
    /// </summary>
    public ProductOptionColor? ColorOption { get; private set; }
    public int? ColorOptionId { get; private set; }
    /// <summary>
    /// گزینه جنس محصول
    /// </summary>
    public ProductOptionMaterial? MaterialOption { get; private set; }
    public int? MaterialOptionId { get; private set; }  
    /// <summary>
    /// فیلدهای اختصاصی
    /// </summary>
    public List<ProductCustomField> ProductCustomFields { get; set; } = new List<ProductCustomField>();
    /// <summary>
    /// نمایش ها
    /// </summary>
    public List<ProductDisplayVariant> ProductDisplayVariants { get; set; } = new List<ProductDisplayVariant>();
    /// <summary>
    /// نظرات
    /// </summary>
    public List<Review> Reviews { get; set; } = new List<Review>();


    public bool SetProductAttributeType(ProductAttributeType productAttributeType)
    {
        if (ProductAttributeTypeId == productAttributeType.Id && ProductAttributeTypeId != 0)
        {
            return true;
        }

        if (ProductAttributeType != null)
        {
            foreach (var attribute in ProductAttributeType.AttributeGroups.SelectMany(c => c.Attributes))
            {
                Attributes = Attributes.Where(c => c.Name != attribute.Value).ToList();
            }
        }
        


        ProductAttributeType = productAttributeType;
        ProductAttributeTypeId = productAttributeType.Id;

        foreach (var attribute in ProductAttributeType.AttributeGroups.SelectMany(c => c.Attributes))
        {
            Attributes.Add(new ProductAttribute
            {
                Name = attribute.Value,
                Slug = attribute.Value.ToLower().Replace(' ', '-'),
                ValueName = "",
                ValueSlug = "",
                Featured = false
            });
        }

        return true;
    }

    public bool SetColorOption(ProductOptionColor? productOptionColor, ProductTypeAttributeGroup? attributeGroup)
    {

        if (ColorOptionId == productOptionColor?.Id)
        {
            return true;
        }

        if (ColorOption != null)
        {
            ProductAttributeType.AttributeGroups =
                ProductAttributeType.AttributeGroups.Where(c => c.Name != ColorOption?.Name).ToList();

            Attributes = Attributes.Where(c => c.Name != EnumHelper<ProductOptionTypeEnum>.GetDisplayValue(ColorOption.Type)).ToList();
        }
       

        ColorOption = productOptionColor;
        ColorOptionId = productOptionColor?.Id;

        if (ColorOption == null || attributeGroup == null)
        {
            return true;
        }

        ProductAttributeType.AttributeGroups.Add(attributeGroup);

        var attribute = new ProductAttribute
        {
            Name = EnumHelper<ProductOptionTypeEnum>.GetDisplayValue(ColorOption.Type),
            Slug = EnumHelper<ProductOptionTypeEnum>.GetDisplayValue(ColorOption.Type).ToLower().Replace(' ', '-'),
            Featured = true,
            ValueName = "",
            ValueSlug = "",
        };
        Attributes.Add(attribute);


        return true;
    }

    public bool SetMaterialOption(ProductOptionMaterial? productOptionMaterial, ProductTypeAttributeGroup? attributeGroup)
    {

        if (MaterialOptionId == productOptionMaterial?.Id)
        {
            return true;
        }

        if (MaterialOption != null)
        {
            ProductAttributeType.AttributeGroups =
                ProductAttributeType.AttributeGroups.Where(c => c.Name != MaterialOption?.Name).ToList();

            Attributes = Attributes.Where(c => c.Name != EnumHelper<ProductOptionTypeEnum>.GetDisplayValue(MaterialOption.Type)).ToList();
        }


        MaterialOption = productOptionMaterial;
        MaterialOptionId = productOptionMaterial?.Id;

        if (MaterialOption == null || attributeGroup == null)
        {
            return true;
        }

        ProductAttributeType.AttributeGroups.Add(attributeGroup);

        var attribute = new ProductAttribute
        {
            Name = EnumHelper<ProductOptionTypeEnum>.GetDisplayValue(MaterialOption.Type),
            Slug = EnumHelper<ProductOptionTypeEnum>.GetDisplayValue(MaterialOption.Type).ToLower().Replace(' ', '-'),
            Featured = true,
            ValueName = "",
            ValueSlug = "",
        };
        Attributes.Add(attribute);


        return true;
    }
}