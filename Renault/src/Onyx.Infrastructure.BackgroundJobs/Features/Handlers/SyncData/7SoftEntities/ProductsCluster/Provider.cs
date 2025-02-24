namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster;

public class Provider
{
    public Guid UniqueId { get; set; }
    public int Code { get; set; }
    public string ProviderLocalizedName { get; set; } = null!;
    public string? ProviderName { get; set; } = null!;
    public string? ProviderLocalizedCode { get; set; }
    public string? Description { get; set; }
}