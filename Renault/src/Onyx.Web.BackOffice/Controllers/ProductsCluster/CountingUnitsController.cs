using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.CountingUnits.Commands.CreateCountingUnit;
using Onyx.Application.Main.ProductsCluster.CountingUnits.Commands.DeleteCountingUnit;
using Onyx.Application.Main.ProductsCluster.CountingUnits.Commands.UpdateCountingUnit;
using Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.BackOffice.GetCountingUnit;
using Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.BackOffice.GetCountingUnits;
using Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.BackOffice.GetCountingUnitsWithPagination;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.CountingUnits.Validators;
using Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.Export.ExportExcelCountingUnits;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;


public class CountingUnitsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<CountingUnitDto>>> GetCountingUnitsWithPagination([FromQuery] GetCountingUnitsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<CountingUnitDto>>> GetAllCountingUnits()
    {
        return await Mediator.Send(new GetAllCountingUnitsQuery());
    }

    [HttpGet("allDropDown")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllCountingUnitDropDownDto>>> GetAllCountingUnitsDropDown()
    {
        return await Mediator.Send(new GetAllCountingUnitsDropDownQuery());
    }


    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<CountingUnitDto?>> GetCountingUnitById(int id)
    {
        return await Mediator.Send(new GetCountingUnitByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelCountingUnitsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "CountingUnits.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateCountingUnitCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateCountingUnitCommand command)
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
        await Mediator.Send(new DeleteCountingUnitCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeCountingUnit([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeCountingUnitCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueCountingUnitLocalizedName([FromQuery] UniqueCountingUnitLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueCountingUnitName([FromQuery] UniqueCountingUnitNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
