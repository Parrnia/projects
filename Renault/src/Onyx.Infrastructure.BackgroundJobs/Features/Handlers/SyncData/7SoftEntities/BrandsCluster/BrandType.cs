
namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.BrandsCluster;

public class BrandType
{
    public Guid UniqueId { get; set; }
    public int Code { get; set; }
    public string BrandTypeName { get; set; } = null!;
    public string BrandTypeLocalizedName { get; set; } = null!;
}