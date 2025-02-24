using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.BrandsCluster.Families.Commands.CreateFamily;
using Onyx.Application.Main.BrandsCluster.Families.Commands.DeleteFamily;
using Onyx.Application.Main.BrandsCluster.Families.Commands.UpdateFamily;
using Onyx.Application.Main.BrandsCluster.Families.Queries.BackOffice;
using Onyx.Application.Main.BrandsCluster.Families.Queries.BackOffice.GetFamilies;
using Onyx.Application.Main.BrandsCluster.Families.Queries.BackOffice.GetFamiliesWithPagination;
using Onyx.Application.Main.BrandsCluster.Families.Queries.BackOffice.GetFamily;
using Onyx.Application.Main.BrandsCluster.Families.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.BrandsCluster.Families.Queries.Export.ExportExcelFamilies;


namespace Onyx.Web.BackOffice.Controllers.BrandsCluster;


public class FamiliesController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<FamilyDto>>> GetFamiliesWithPagination([FromQuery] GetFamiliesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<FamilyDto>>> GetAllFamilies()
    {
        return await Mediator.Send(new GetAllFamiliesQuery());
    }

    [HttpGet("brand{brandId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<FamilyDto>>> GetFamiliesByBrandId(int brandId)
    {
        return await Mediator.Send(new GetFamiliesByBrandIdQuery(brandId));
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FamilyDto?>> GetFamilyById(int id)
    {
        return await Mediator.Send(new GetFamilyByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelFamiliesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Families.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateFamilyCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateFamilyCommand command)
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
        await Mediator.Send(new DeleteFamilyCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeFamily([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeFamilyCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueFamilyLocalizedName([FromQuery] UniqueFamilyLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueFamilyName([FromQuery] UniqueFamilyNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
