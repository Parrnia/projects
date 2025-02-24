namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.BrandsCluster;

public class Vehicle
{
    public Guid UniqueId { get; set; }
    public int Code { get; set; }
    public string VehicleLocalizedName { get; set; } = null!;
    public string? VehicleEnglishName { get; set; } = null!;
    public Guid VehicleTypeId { get; set; }
}