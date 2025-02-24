using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.LayoutsCluster.Carousels.Queries.FrontOffice;
using Onyx.Application.Main.LayoutsCluster.Carousels.Queries.FrontOffice.GetCarousels;

namespace Onyx.Web.FrontOffice.Controllers.LayoutsCluster;

public class CarouselsController : ApiControllerBase
{

    [HttpGet("all")]
    public async Task<ActionResult<List<CarouselDto>>> GetAllCarousels()
    {
        return await Mediator.Send(new GetAllCarouselsQuery());
    }

}
