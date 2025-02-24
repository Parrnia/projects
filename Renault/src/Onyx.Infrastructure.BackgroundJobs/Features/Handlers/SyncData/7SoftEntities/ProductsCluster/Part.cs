

namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster;
public class Part
{
    public Guid UniqueId { get; set; }
    public int Code { get; set; }
    public string? PartNo { get; set; }
    public Guid? PartProviderId { get; set; }
    public string PartLocalizedName { get; set; } = null!;
    public string? PartName { get; set; } = null!;
    public string? OldProductNo { get; set; }
    public Guid PartGroupId { get; set; }
    public Guid? CountryId { get; set; }
    public Guid PartBrandId { get; set; }
    public string? PartCatalog { get; set; }
    public Guid? MainCountingUnitId { get; set; }
    public Guid? CommonCountingUnitId { get; set; }
    public double OrderRate { get; set; }
    public double MaxOrderQty { get; set; }
    public double MinOrderQty { get; set; }
    public Guid? PartTypeId { get; set; }
    public Guid? PartStatusId { get; set; }
    public decimal? PartHeight { get; set; }
    public decimal? PartWidth { get; set; }
    public decimal? PartLength { get; set; }
    public decimal? PartNetWeight { get; set; }
    public decimal? PartGrossWeight { get; set; }
    public decimal? PartVolumeWeight { get; set; }
    public int? Mileage { get; set; }
    public int? Duration { get; set; }
    public double SaftyStockQty { get; set; }
    public double MinStockQty { get; set; }
    public double MaxStockQty { get; set; }
    public bool AllVehicleModels { get; set; }
}
public class PartResponse
{
    public int AddStatus { get; set; }
    public string ReturnKey { get; set; }
    public string Message { get; set; }
    public List<Part> ReturnModel { get; set; } = new List<Part>();
}