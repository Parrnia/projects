using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.FrontOffice;
using Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.FrontOffice.GetBlockBanners;

namespace Onyx.Web.FrontOffice.Controllers.LayoutsCluster;

public class BlockBannersController : ApiControllerBase
{

    [HttpGet("all")]
    public async Task<ActionResult<List<BlockBannerDto>>> GetAllBlockBanners()
    {
        return await Mediator.Send(new GetAllBlockBannersQuery());
    }

}
