
namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster;

public class CountingUnit
{
    public Guid UniqueId { get; set; }
    public int Code { get; set; }
    public string CountingUnitLocalizedName { get; set; } = null!;
    public string CountingUnitName { get; set; } = null!;
    public bool IsDecimal { get; set; }
    public int CountingUnitTypeId { get; set; }
}