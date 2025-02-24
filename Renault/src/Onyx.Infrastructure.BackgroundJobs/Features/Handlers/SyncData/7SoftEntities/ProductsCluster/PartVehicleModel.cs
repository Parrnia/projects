namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster
{
    public class PartVehicleModel
    {
        public Guid UniqueId { get; set; }
        public int Code { get; set; }
        public Guid PartId { get; set; }
        public Guid VehicleModelId { get; set; }
    }
    public class PartVehicleResponse
    {
        public int AddStatus { get; set; }
        public string ReturnKey { get; set; }
        public string Message { get; set; }
        public List<PartVehicleModel> ReturnModel { get; set; } = new List<PartVehicleModel>();
    }
}
