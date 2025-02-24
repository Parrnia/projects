using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.InfoCluster.AboutUsInfo.Queries.FrontOffice.GetAboutUs;

namespace Onyx.Web.FrontOffice.Controllers.InfoCluster;


public class AboutUsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<AboutUsDto?>> GetAboutUs()
    {
        return await Mediator.Send(new GetAboutUsQuery());
    }
}
