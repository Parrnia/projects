using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModel.GetModelById;
public class ModelByIdDto : IMapFrom<Model>
{
    public ModelByIdDto()
    {
        Kinds = new List<KindDto>();
    }
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public IList<KindDto> Kinds { get; set; }
}
public class KindDto : IMapFrom<Kind>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
}