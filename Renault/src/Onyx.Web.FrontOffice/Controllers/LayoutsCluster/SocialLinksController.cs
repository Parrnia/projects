using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Queries.FrontOffice.GetSocialLinks.GetAllSocialLinks;

namespace Onyx.Web.FrontOffice.Controllers.LayoutsCluster;


public class SocialLinksController : ApiControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<List<AllSocialLinkDto>>> GetAllSocialLinks()
    {
        return await Mediator.Send(new GetAllSocialLinksQuery());
    }
}
