using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.BrandsCluster.Vehicles.Commands.CreateVehicle;
using Onyx.Application.Main.BrandsCluster.Vehicles.Commands.DeleteVehicle;
using Onyx.Application.Main.BrandsCluster.Vehicles.Commands.UpdateVehicle;
using Onyx.Application.Main.BrandsCluster.Vehicles.Queries.BackOffice;
using Onyx.Application.Main.BrandsCluster.Vehicles.Queries.BackOffice.GetVehicle;
using Onyx.Application.Main.BrandsCluster.Vehicles.Queries.BackOffice.GetVehicles;
using Onyx.Application.Main.BrandsCluster.Vehicles.Queries.BackOffice.GetVehiclesWithPagination;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.BrandsCluster.Vehicles.Validators;
using Onyx.Application.Main.BrandsCluster.Vehicles.Queries.Export.ExportExcelVehicles;

namespace Onyx.Web.BackOffice.Controllers.BrandsCluster;


public class VehiclesController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<VehicleByIdDto>>> GetVehiclesWithPagination([FromQuery] GetAllVehiclesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<VehicleDto>>> GetAllVehicles()
    {
        return await Mediator.Send(new GetAllVehiclesQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<VehicleByIdDto?>> GetVehicleById(int id)
    {
        return await Mediator.Send(new GetVehicleByIdQuery(id));
    }
    [HttpGet("customer{customerId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<VehicleByIdDto>>> GetVehiclesByCustomerId(Guid customerId)
    {
        return await Mediator.Send(new GetVehiclesByCustomerIdQuery(customerId));
    }

    [HttpGet("kind{kindId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<VehicleByIdDto>>> GetVehiclesByKindId(int kindId)
    {
        return await Mediator.Send(new GetVehiclesByKindIdQuery(kindId));
    }

    [HttpGet("vin{vinNumber}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<VehicleForVinDto?>> GetVehicleByVinNumber(string vinNumber)
    {
        return await Mediator.Send(new GetVehicleByVinNumberQuery(vinNumber));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelVehiclesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Vehicles.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateVehicleCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateVehicleCommand command)
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
        await Mediator.Send(new DeleteVehicleCommand(id, null));

        return NoContent();
    }
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeVehicle([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeVehicleCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueVehicleKindId")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueVehicleKindId([FromQuery] UniqueVehicleKindIdValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueVehicleVinNumber")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueVehicleVinNumber([FromQuery] UniqueVehicleVinNumberValidator query)
    {
        return await Mediator.Send(query);
    }
}
