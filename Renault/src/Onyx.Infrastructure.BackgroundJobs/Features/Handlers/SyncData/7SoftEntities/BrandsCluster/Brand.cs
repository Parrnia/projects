namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.BrandsCluster;
public class Brand
{
    public Guid UniqueId { get; set; }
    public int Code { get; set; }
    public Guid? BrandTypeId { get; set; }
    public string BrandLocalizedName { get; set; } = null!;
    public string BrandEnglishName { get; set; } = null!;
    public Guid? BrandLogoId { get; set; }
}
