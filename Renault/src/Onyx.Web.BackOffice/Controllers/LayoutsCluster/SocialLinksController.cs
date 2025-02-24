using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Commands.CreateSocialLink;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Commands.DeleteSocialLink;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Commands.UpdateSocialLink;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Queries.BackOffice;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Queries.BackOffice.GetSocialLink;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Queries.BackOffice.GetSocialLinks;
using Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Queries.Export.ExportExcelCustomerTickets;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.LayoutsCluster;


public class SocialLinksController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<SocialLinkDto>>> GetAllSocialLinks()
    {
        return await Mediator.Send(new GetAllSocialLinksQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<SocialLinkDto?>> GetSocialLinkById(int id)
    {
        return await Mediator.Send(new GetSocialLinkByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelSocialLinksQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "SocialLinks.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateSocialLinkCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int urlId, UpdateSocialLinkCommand command)
    {
        if (urlId != command.Id)
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
        await Mediator.Send(new DeleteSocialLinkCommand(id));

        return NoContent();
    }
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeSocialLink([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeSocialLinkCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
