
namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.InfoCluster;

public class Country
{
    public Guid UniqueId { get; set; }
    public int Code { get; set; }
    public string CountryLocalizedName { get; set; } = null!;
    public string CountryEnglishName { get; set; } = null!;
}