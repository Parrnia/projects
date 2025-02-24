
namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.CategoriesCluster;

public class PartGroup
{
    public Guid UniqueId { get; set; }
    public int Code { get; set; }
    public string PartGroupLocalizedName { get; set; } = null!;
    public string PartGroupName { get; set; } = null!;
    public string? PartGroupNo { get; set; }
    public Guid? ParentPartGroupId { get; set; }
}