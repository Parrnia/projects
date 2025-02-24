using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Queries.BackOffice;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Queries.BackOffice.GetFooterLinkContainer;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Queries.BackOffice.GetFooterLinkContainers;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Queries.FrontOffice.GetFooterLinkContainersWithPagination.GetFooterLinkContainersWithPagination;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;

namespace Onyx.Web.FrontOffice.Controllers.LayoutsCluster;


public class FooterLinkContainersController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<FooterLinkContainerWithPaginationDto>>> GetFooterLinkContainersWithPagination([FromQuery] GetFooterLinkContainersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<FooterLinkContainerDto>>> GetAllFooterLinkContainers()
    {
        return await Mediator.Send(new GetAllFooterLinkContainersQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FooterLinkContainerDto?>> GetFooterLinkContainerById(int id)
    {
        return await Mediator.Send(new GetFooterLinkContainerByIdQuery(id));
    }
}
