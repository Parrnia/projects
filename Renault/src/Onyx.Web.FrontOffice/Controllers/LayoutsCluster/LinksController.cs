using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.LayoutsCluster.HeaderCluster.Links.Queries.FrontOffice.GetLinks.GetAllLinks;
using Onyx.Application.Main.LayoutsCluster.HeaderCluster.Links.Queries.FrontOffice.GetLinks.GetFirstLayerLinks;

namespace Onyx.Web.FrontOffice.Controllers.LayoutsCluster;

public class LinksController : ApiControllerBase
{

    [HttpGet("all")]
    public async Task<ActionResult<List<AllLinkDto>>> GetAllLinks()
    {
        return await Mediator.Send(new GetAllLinksQuery());
    }
    [HttpGet("firstLayer")]
    public async Task<ActionResult<List<FirstLayerLinkDto>>> GetFirstLayerLinks()
    {
        return await Mediator.Send(new GetFirstLayerLinksQuery());
    }
}
