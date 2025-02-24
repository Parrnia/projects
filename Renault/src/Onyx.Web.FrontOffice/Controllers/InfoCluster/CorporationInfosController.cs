using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.InfoCluster.CorporationInfos.Queries.FrontOffice.GetCorporationInfo;

namespace Onyx.Web.FrontOffice.Controllers.InfoCluster;


public class CorporationInfosController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CorporationInfoDto?>> GetCorporationInfo()
    {
        return await Mediator.Send(new GetCorporationInfoQuery());
    }
}
