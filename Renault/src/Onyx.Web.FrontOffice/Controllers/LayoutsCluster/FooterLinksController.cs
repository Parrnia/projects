using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice.GetFooterLink;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice.GetFooterLinks;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice.GetFooterLinksWithPagination;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;

namespace Onyx.Web.FrontOffice.Controllers.LayoutsCluster;


public class FooterLinksController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<FooterLinkDto>>> GetFooterLinksWithPagination([FromQuery] GetFooterLinksWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<FooterLinkDto>>> GetAllFooterLinks()
    {
        return await Mediator.Send(new GetAllFooterLinksQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FooterLinkDto?>> GetFooterLinkById(int id)
    {
        return await Mediator.Send(new GetFooterLinkByIdQuery(id));
    }
}
