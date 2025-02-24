using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.LayoutsCluster.Themes.Commands.CreateTheme;
using Onyx.Application.Main.LayoutsCluster.Themes.Commands.DeleteTheme;
using Onyx.Application.Main.LayoutsCluster.Themes.Commands.UpdateTheme;
using Onyx.Application.Main.LayoutsCluster.Themes.Queries.BackOffice;
using Onyx.Application.Main.LayoutsCluster.Themes.Queries.BackOffice.GetTheme;
using Onyx.Application.Main.LayoutsCluster.Themes.Queries.BackOffice.GetThemes;
using Onyx.Application.Main.LayoutsCluster.Themes.Queries.BackOffice.GetThemesWithPagination;
using Onyx.Application.Main.LayoutsCluster.Themes.Validators;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.LayoutsCluster;

public class ThemesController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<ThemeDto>>> GetThemesWithPagination([FromQuery] GetThemesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ThemeDto>>> GetAllThemes()
    {
        return await Mediator.Send(new GetAllThemesQuery());
    }


    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ThemeDto?>> GetThemeById(int id)
    {
        return await Mediator.Send(new GetThemeByIdQuery(id));
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateThemeCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int urlId, UpdateThemeCommand command)
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
        await Mediator.Send(new DeleteThemeCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeTheme([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeThemeCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueTitle")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueThemeTitle([FromQuery] UniqueThemeTitleValidator query)
    {
        return await Mediator.Send(query);
    }
}
