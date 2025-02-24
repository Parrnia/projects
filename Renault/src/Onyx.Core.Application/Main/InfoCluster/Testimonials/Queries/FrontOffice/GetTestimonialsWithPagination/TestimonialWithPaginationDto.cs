using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Queries.FrontOffice.GetTestimonialsWithPagination;
public class TestimonialWithPaginationDto : IMapFrom<Testimonial>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Position { get; set; } = null!;
    public Guid Avatar { get; set; }
    public int Rating { get; set; }
    public string Review { get; set; } = null!;
}
