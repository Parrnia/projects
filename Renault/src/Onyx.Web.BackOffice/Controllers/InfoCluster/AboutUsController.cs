using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.InfoCluster.AboutUsInfo.Commands.CreateAboutUs;
using Onyx.Application.Main.InfoCluster.AboutUsInfo.Commands.DeleteAboutUs;
using Onyx.Application.Main.InfoCluster.AboutUsInfo.Commands.UpdateAboutUs;
using Onyx.Application.Main.InfoCluster.AboutUsInfo.Queries.BackOffice.GetAboutUs;
using Onyx.Application.Main.InfoCluster.AboutUsInfo.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;

namespace Onyx.Web.BackOffice.Controllers.InfoCluster;


public class AboutUsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AboutUsDto>>> GetAllAboutUs()
    {
        return await Mediator.Send(new GetAllAboutUsQuery());
    }
    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateAboutUsCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int urlId, UpdateAboutUsCommand command)
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
        await Mediator.Send(new DeleteAboutUsCommand(id));

        return NoContent();
    }
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeAboutUs([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeAboutUsCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueTitle")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueAboutUsTitle([FromQuery] UniqueAboutUsTitleValidator query)
    {
        return await Mediator.Send(query);
    }
}
