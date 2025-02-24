using Onyx.Domain.Entities.BrandsCluster;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Domain.Entities.InfoCluster;
public class Country : BaseAuditableEntity
{
    /// <summary>
    /// کلید اصلی در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftCountryId { get; set; }
    /// <summary>
    /// فیلد شمارنده
    /// </summary>
    public int Code { get; set; }
    /// <summary>
    /// نام فارسی
    /// </summary>
    public string LocalizedName { get; set; } = null!;
    /// <summary>
    /// نام لاتین
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// آدرس ها
    /// </summary>
    public List<Address> Addresses { get; set; } = new List<Address>();
    /// <summary>
    /// برندهای کالا
    /// </summary>
    public List<ProductBrand> ProductBrands { get; set; } = new List<ProductBrand>();
    /// <summary>
    /// برندهای خودرو
    /// </summary>
    public List<VehicleBrand> VehicleBrands { get; set; } = new List<VehicleBrand>();
    /// <summary>
    /// محصولات
    /// </summary>
    public List<Product> Products { get; set; } = new List<Product>();
}
