using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.Badges.Commands.CreateBadge;
using Onyx.Application.Main.ProductsCluster.Badges.Commands.DeleteBadge;
using Onyx.Application.Main.ProductsCluster.Badges.Commands.UpdateBadge;
using Onyx.Application.Main.ProductsCluster.Badges.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.Badges.Queries.BackOffice.GetBadge;
using Onyx.Application.Main.ProductsCluster.Badges.Queries.BackOffice.GetBadges;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.Badges.Commands.AddRangeProductAttributeOptionBadges;
using Onyx.Application.Main.ProductsCluster.Badges.Validators;
using Onyx.Application.Main.ProductsCluster.Badges.Queries.Export.ExportExcelBadges;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;


public class BadgesController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<BadgeDto>>> GetAllBadges()
    {
        return await Mediator.Send(new GetAllBadgesQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<BadgeDto?>> GetBadgeById(int id)
    {
        return await Mediator.Send(new GetBadgeByIdQuery(id));
    }

    [HttpGet("option{optionId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<BadgeDto>>> GetAllBadgesByOptionId(int optionId)
    {
        return await Mediator.Send(new GetAllBadgesByOptionIdQuery(optionId));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelBadgesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Badges.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateBadgeCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateBadgeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("addRange")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> AddRange(AddRangeProductAttributeOptionBadgesCommand command)
    {

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteBadgeCommand(id));

        return NoContent();
    }
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeBadge([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeBadgeCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("rangeFromOption{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeBadgeFromAttributeOption(int id,[FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeBadgeFromAttributeOptionCommand() { Ids = ids, ProductAttributeOptionId = id};
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueBadgeValue")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueBadgeValue([FromQuery] UniqueBadgeValueValidator query)
    {
        return await Mediator.Send(query);
    }
}
