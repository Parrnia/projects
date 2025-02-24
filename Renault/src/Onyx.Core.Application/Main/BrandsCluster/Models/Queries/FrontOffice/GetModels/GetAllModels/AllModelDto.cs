using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModels.GetAllModels;
public class AllModelDto : IMapFrom<Model>
{
    public AllModelDto()
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
    public bool IsActive { get; set; }
}