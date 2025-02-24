namespace Onyx.Domain.Entities.BrandsCluster;
public class Family : BaseAuditableEntity
{
    /// <summary>
    /// کلید اصلی در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftFamilyId { get; set; }
    /// <summary>
    /// نام فارسی
    /// </summary>
    public string LocalizedName { get; set; } = null!;
    /// <summary>
    /// نام لاتین
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// برند
    /// </summary>
    public VehicleBrand VehicleBrand { get; set; } = null!;
    public int VehicleBrandId { get; set; }
    /// <summary>
    /// مدل ها
    /// </summary>
    public List<Model> Models { get; set; } = new List<Model>();
}
