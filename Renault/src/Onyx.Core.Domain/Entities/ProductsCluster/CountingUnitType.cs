namespace Onyx.Domain.Entities.ProductsCluster;
public class CountingUnitType : BaseAuditableEntity
{
    /// <summary>
    /// کلید اصلی در دیتابیس قبلی
    /// </summary>
    public Guid? Related7SoftCountingUnitTypeId { get; set; }
    /// <summary>
    /// فیلد شمارنده
    /// </summary>
    public int Code { get; set; }
    /// <summary>
    /// نام لاتین
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// نام فارسی
    /// </summary>
    public string LocalizedName { get; set; } = null!;
    /// <summary>
    /// واحدهای شمارش مرتبط
    /// </summary>
    public List<CountingUnit> CountingUnits { get; set; } = new List<CountingUnit>();
}
