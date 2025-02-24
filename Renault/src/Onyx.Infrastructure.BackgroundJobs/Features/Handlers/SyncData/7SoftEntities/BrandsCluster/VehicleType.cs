namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.BrandsCluster;

public class VehicleType
{
    public Guid UniqueId { get; set; }
    public int Code { get; set; }
    public string VehicleTypeLocalizedName { get; set; } = null!;
    public string? VehicleTypeEnglishName { get; set; } = null!;
    public Guid BrandId { get; set; }
}