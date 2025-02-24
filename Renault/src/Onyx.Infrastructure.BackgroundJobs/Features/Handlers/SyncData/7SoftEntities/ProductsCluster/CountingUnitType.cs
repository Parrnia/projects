namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster;

public class CountingUnitType
{
    public Guid UniqueId { get; set; }
    public int Code { get; set; }
    public string CountingUnitTypeName { get; set; } = null!;
    public string CountingUnitTypeLocalizedName { get; set; } = null!;
}