using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.BackOffice;
public class ModelDto : IMapFrom<Model>
{
    public int Id { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int FamilyId { get; set; }
    public string FamilyName { get; set; } = null!;
    public bool IsActive { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Model, ModelDto>()
            .ForMember(d => d.FamilyId, opt => opt.MapFrom(s => s.FamilyId))
            .ForMember(d => d.FamilyName, opt => opt.MapFrom(s => s.Family.LocalizedName));
    }
}
