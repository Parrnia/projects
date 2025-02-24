using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.BackOffice;
public class KindDto : IMapFrom<Kind>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int ModelId { get; set; }
    public bool IsActive { get; set; }
    public string ModelName { get; set; } = null!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Kind, KindDto>()
            .ForMember(d => d.ModelId, opt => opt.MapFrom(s => s.ModelId))
            .ForMember(d => d.ModelName, opt => opt.MapFrom(s => s.Model.LocalizedName));
    }
}
