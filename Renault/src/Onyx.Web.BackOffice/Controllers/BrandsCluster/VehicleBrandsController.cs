using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.BrandsCluster.VehicleBrands.Commands.CreateVehicleBrand;
using Onyx.Application.Main.BrandsCluster.VehicleBrands.Commands.DeleteVehicleBrand;
using Onyx.Application.Main.BrandsCluster.VehicleBrands.Commands.UpdateVehicleBrand;
using Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.BackOffice;
using Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.BackOffice.GetVehicleBrand;
using Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.BackOffice.GetVehicleBrands;
using Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.Export.ExportExcelVehicleBrands;
using Onyx.Application.Main.BrandsCluster.VehicleBrands.Validators;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.BrandsCluster;


public class VehicleBrandsController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<VehicleBrandDto>>> GetAllVehicleBrands()
    {
        return await Mediator.Send(new GetAllVehicleBrandsQuery());
    }

    [HttpGet("allDropDown")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllVehicleBrandDropDownDto>>> GetAllVehicleBrandsDropDown()
    {
        return await Mediator.Send(new GetAllVehicleBrandsDropDownQuery());
    }


    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<VehicleBrandDto?>> GetVehicleBrandById(int id)
    {
        return await Mediator.Send(new GetVehicleBrandByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelVehicleBrandsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "VehicleBrands.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateVehicleBrandCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int urlId, UpdateVehicleBrandCommand command)
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
        await Mediator.Send(new DeleteVehicleBrandCommand(id));

        return NoContent();
    }
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeVehicleBrand([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeVehicleBrandCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueVehicleBrandLocalizedName([FromQuery] UniqueVehicleBrandLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueVehicleBrandName([FromQuery] UniqueVehicleBrandNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
