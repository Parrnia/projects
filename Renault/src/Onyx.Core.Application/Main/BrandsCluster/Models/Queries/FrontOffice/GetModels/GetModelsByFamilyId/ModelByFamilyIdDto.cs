using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModels.GetModelsByFamilyId;
public class ModelByFamilyIdDto : IMapFrom<Model>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
}
