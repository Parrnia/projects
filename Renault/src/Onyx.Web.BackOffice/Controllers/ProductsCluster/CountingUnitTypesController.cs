using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Commands.CreateCountingUnitType;
using Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Commands.DeleteCountingUnitType;
using Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Commands.UpdateCountingUnitType;
using Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.BackOffice.GetCountingUnitType;
using Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.BackOffice.GetCountingUnitTypes;
using Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.BackOffice.GetCountingUnitTypesWithPagination;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Validators;
using Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.Export.ExportExcelCountingUnitTypes;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;


public class CountingUnitTypesController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<CountingUnitTypeDto>>> GetCountingUnitTypesWithPagination([FromQuery] GetCountingUnitTypesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<CountingUnitTypeDto>>> GetAllCountingUnitTypes()
    {
        return await Mediator.Send(new GetAllCountingUnitTypesQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<CountingUnitTypeDto?>> GetCountingUnitTypeById(int id)
    {
        return await Mediator.Send(new GetCountingUnitTypeByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelCountingUnitTypesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "CountingUnitTypes.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateCountingUnitTypeCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateCountingUnitTypeCommand command)
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
        await Mediator.Send(new DeleteCountingUnitTypeCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeCountingUnitType([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeCountingUnitTypeCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueCountingUnitTypeLocalizedName([FromQuery] UniqueCountingUnitTypeLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueCountingUnitTypeName([FromQuery] UniqueCountingUnitTypeNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
