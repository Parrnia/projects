using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.Badges.Queries.FrontOffice.GetBadge;
using Onyx.Application.Main.ProductsCluster.Badges.Queries.FrontOffice.GetBadges;

namespace Onyx.Web.FrontOffice.Controllers.ProductsCluster;


public class BadgesController : ApiControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<List<AllBadgeDto>>> GetAllBadges()
    {
        return await Mediator.Send(new GetAllBadgesQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BadgeByIdDto?>> GetBadgeById(int id)
    {
        return await Mediator.Send(new GetBadgeByIdQuery(id));
    }
}
