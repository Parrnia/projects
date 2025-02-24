using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.BackOffice;
public class CountingUnitDto : IMapFrom<CountingUnit>
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string LocalizedName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsDecimal { get; set; }
    public int CountingUnitTypeId { get; set; }
    public string CountingUnitTypeName { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CountingUnit, CountingUnitDto>()
            .ForMember(d => d.CountingUnitTypeId, opt => opt.MapFrom(s => s.CountingUnitTypeId))
            .ForMember(d => d.CountingUnitTypeName, opt => opt.MapFrom(s => s.CountingUnitType != null ? s.CountingUnitType.LocalizedName : null));
    }
}
