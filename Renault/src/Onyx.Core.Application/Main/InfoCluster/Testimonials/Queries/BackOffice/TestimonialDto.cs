using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Queries.BackOffice;
public class TestimonialDto : IMapFrom<Testimonial>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Position { get; set; } = null!;
    public Guid Avatar { get; set; }
    public int Rating { get; set; }
    public string Review { get; set; } = null!;
    public string AboutUsTitle { get; set; } = null!;
    public bool IsActive { get; set; }
    public int AboutUsId { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Testimonial, TestimonialDto>()
            .ForMember(d => d.AboutUsTitle, opt => opt.MapFrom(s => s.AboutUs.Title))
            .ForMember(d => d.AboutUsId, opt => opt.MapFrom(s => s.AboutUsId));
    }
}
