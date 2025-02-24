using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Domain.Entities.BrandsCluster;
public class Kind : BaseAuditableEntity
{
    /// <summary>
    /// کلید اصلی در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftKindId { get; set; }
    /// <summary>
    /// نام فارسی
    /// </summary>
    public string LocalizedName { get; set; } = null!;
    /// <summary>
    /// نام لاتین
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// مدل
    /// </summary>
    public Model Model { get; set; } = null!;
    public int ModelId { get; set; }
    /// <summary>
    /// کالاهای مرتبط
    /// </summary>
    public List<Product> Products { get; set; } = new List<Product>();
    /// <summary>
    /// vin های مرتبط
    /// </summary>
    public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
