using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Domain.Entities.BrandsCluster;
public class VehicleBrand : BaseAuditableEntity
{
    /// <summary>
    /// کلید اصلی در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftBrandId { get; set; }
    /// <summary>
    /// تصویر لوگو
    /// </summary>
    public Guid? BrandLogo { get; set; } = null!;
    /// <summary>
    /// نام فارسی
    /// </summary>
    public string LocalizedName { get; set; } = null!;
    /// <summary>
    /// نام لاتین
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// فیلد شمارنده
    /// </summary>
    public int Code { get; set; }
    /// <summary>
    /// عنوان کوتاه
    /// </summary>
    public string Slug { get; set; } = null!;
    /// <summary>
    /// کشور
    /// </summary>
    public Country? Country { get; set; }
    public int? CountryId { get; set; }
    /// <summary>
    /// لیست زیرمجموعه های فامیلی مربوط به خودرو
    /// </summary>
    public List<Family> Families { get; set; } = new List<Family>();
}
