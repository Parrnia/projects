using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Services;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Queries.BackOffice;
public class MaxCreditDto : IMapFrom<MaxCredit>
{
    public int Id { get; set; }
    public string Date { get; set; } = null!;
    public decimal Value { get; set; }
    public string ModifierUserName { get; set; } = null!;
    public Guid ModifierUserId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<MaxCredit, MaxCreditDto>()
            .ForMember(d => d.Date,
                opt =>
                    opt.MapFrom(s => s.Date.ToPersianDate()));
    }
}