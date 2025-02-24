using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Services;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Credits.Queries.GetCreditsWithPagination;
public class CreditDto : IMapFrom<Credit>
{
    public int Id { get; set; }
    public string Date { get; set; } = null!;
    public decimal Value { get; set; }
    public string ModifierUserName { get; set; } = null!;
    public Guid ModifierUserId { get; set; }
    public string? OrderToken { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Credit, CreditDto>()
            .ForMember(d => d.Date,
                opt =>
                    opt.MapFrom(s => s.Date.ToPersianDate()));
    }
}