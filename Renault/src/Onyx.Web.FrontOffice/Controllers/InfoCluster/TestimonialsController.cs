using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.InfoCluster.Testimonials.Queries.FrontOffice.GetTestimonial;
using Onyx.Application.Main.InfoCluster.Testimonials.Queries.FrontOffice.GetTestimonials;
using Onyx.Application.Main.InfoCluster.Testimonials.Queries.FrontOffice.GetTestimonialsWithPagination;

namespace Onyx.Web.FrontOffice.Controllers.InfoCluster;


public class TestimonialsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<TestimonialWithPaginationDto>>> GetTestimonialsWithPagination([FromQuery] GetTestimonialsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AllTestimonialDto>>> GetAllTestimonials()
    {
        return await Mediator.Send(new GetAllTestimonialsQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TestimonialByIdDto?>> GetTestimonialById(int id)
    {
        return await Mediator.Send(new GetTestimonialByIdQuery(id));
    }
}
