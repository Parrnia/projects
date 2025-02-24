using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.BrandsCluster;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.BackOffice;
public class KindForVinDto : IMapFrom<Kind>
{
    public int Id { get; set; }
    //public string LocalizedNameForVehicle { get; set; } = null!;
    //public string NameForVehicle { get; set; } = null!;
    //public void Mapping(Profile profile)
    //{
    //    profile.CreateMap<Kind, KindForVinDto>()
    //        .ForMember(d => d.LocalizedNameForVehicle,
    //            opt => opt.MapFrom(s => GetVehicleLocalizedName(s)))
    //        .ForMember(d => d.NameForVehicle,
    //            opt => opt.MapFrom(s => GetVehicleName(s)));
    //}

    //private string GetVehicleLocalizedName(Kind kind)
    //{
    //    var vehicleName = kind.Model.Family.Brand.LocalizedName
    //                      + kind.Model.Family.LocalizedName
    //                      + kind.Model.LocalizedName
    //                      + kind.LocalizedName;
    //    return vehicleName;
    //}
    //private string GetVehicleName(Kind kind)
    //{
    //    var vehicleName = kind.Model.Family.Brand.Name
    //                      + kind.Model.Family.Name
    //                      + kind.Model.Name
    //                      + kind.Name;
    //    return vehicleName;
    //}
}

