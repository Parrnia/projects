using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductKinds.Queries.BackOffice;

public class AllProductKindByProductIdDto : IMapFrom<ProductKind>
{
    public int Id { get; set; }
    public int KindId { get; set; }
    public string KindName { get; set; } = null!;
    public int ProductId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductKind, AllProductKindByProductIdDto>()
            .ForMember(d => d.KindName, opt => opt.MapFrom(s => s.Kind.LocalizedName))
            .ForMember(d => d.KindId, opt => opt.MapFrom(s => s.Kind.Id));

    }
}
