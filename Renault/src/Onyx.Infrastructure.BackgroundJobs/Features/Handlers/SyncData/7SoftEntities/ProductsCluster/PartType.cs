
namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster;

public class PartType
{
    public Guid UniqueId { get; set; }
    public int Code { get; set; }
    public string PartTypeName { get; set; } = null!;
    public string PartTypeLocalizedName { get; set; } = null!;
}