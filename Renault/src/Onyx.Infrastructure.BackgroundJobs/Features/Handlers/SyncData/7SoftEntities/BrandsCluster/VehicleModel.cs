
namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.BrandsCluster;

public class VehicleModel
{
    public Guid UniqueId { get; set; }
    public int Code { get; set; }
    public string VehicleModelLocalizeName { get; set; } = null!;
    public string? VehicleModelEnglishName { get; set; } = null!;
    public Guid VehicleId { get; set; }
}