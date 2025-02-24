using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.FrontOffice.GetFamiliesWithPagination.GetAllFamiliesWithPagination;
public class AllFamilyWithPaginationDto : IMapFrom<Family>
{
    public AllFamilyWithPaginationDto()
    {
        Models = new List<ModelDto>();
    }
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public IList<ModelDto> Models { get; set; }
}
public class ModelDto : IMapFrom<Model>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
}