using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Commands.CreateFooterLink;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Commands.DeleteFooterLink;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Commands.UpdateFooterLink;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice.GetFooterLink;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice.GetFooterLinks;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice.GetFooterLinksWithPagination;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.Export.ExportExcelFooterLinks;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.LayoutsCluster;


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
    [HttpGet("footerLinkContainerId{footerLinkContainerId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<FooterLinkDto>>> GetFooterLinksByFooterLinkContainerId(int footerLinkContainerId)
    {
        return await Mediator.Send(new GetFooterLinksByFooterLinkContainerIdQuery(footerLinkContainerId));
    }
    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FooterLinkDto?>> GetFooterLinkById(int id)
    {
        return await Mediator.Send(new GetFooterLinkByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelFooterLinksQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "FooterLinks.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateFooterLinkCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateFooterLinkCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    [HttpDelete("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteFooterLinkCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeFooterLink([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeFooterLinkCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

}
