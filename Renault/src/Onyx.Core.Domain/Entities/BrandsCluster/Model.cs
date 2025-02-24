namespace Onyx.Domain.Entities.BrandsCluster;
public class Model : BaseAuditableEntity
{
    /// <summary>
    /// کلید اصلی در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftModelId { get; set; }
    /// <summary>
    /// نام فارسی
    /// </summary>
    public string LocalizedName { get; set; } = null!;
    /// <summary>
    /// نام لاتین
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// خانواده
    /// </summary>
    public Family Family { get; set; } = null!;
    public int FamilyId { get; set; }
    /// <summary>
    /// نوع خودروها
    /// </summary>
    public List<Kind> Kinds { get; set; } = new List<Kind>();
}
