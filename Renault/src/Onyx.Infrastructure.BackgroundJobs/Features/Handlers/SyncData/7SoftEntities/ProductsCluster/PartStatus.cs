namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster;

public class PartStatus
{
    public Guid UniqueId { get; set; }
    public int Code { get; set; }
    public string PartStatusName { get; set; } = null!;
    public string PartStatusLocalizedName { get; set; } = null!;
}