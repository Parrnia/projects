using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.BrandsCluster.Vehicles.Commands.CreateVehicle;
using Onyx.Application.Main.BrandsCluster.Vehicles.Commands.DeleteVehicle;
using Onyx.Application.Main.BrandsCluster.Vehicles.Commands.UpdateVehicle;
using Onyx.Application.Main.BrandsCluster.Vehicles.Queries.FrontOffice.GetVehicle.GetVehicleById;
using Onyx.Application.Main.BrandsCluster.Vehicles.Queries.FrontOffice.GetVehicle.GetVehicleByVinNumber;
using Onyx.Application.Main.BrandsCluster.Vehicles.Queries.FrontOffice.GetVehicles.GetVehiclesByCustomerId;
using Onyx.Application.Main.BrandsCluster.Vehicles.Queries.FrontOffice.GetVehicles.GetVehiclesByKindId;
using Onyx.Web.FrontOffice.Authorization;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared;

namespace Onyx.Web.FrontOffice.Controllers.BrandsCluster;

public class VehiclesController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleByIdDto?>> GetVehicleById(int id)
    {
        return await Mediator.Send(new GetVehicleByIdQuery(id));
    }

    [HttpGet("kind{kindId}")]
    public async Task<ActionResult<List<VehicleByKindIdDto>>> GetVehiclesByKindId(int kindId)
    {
        return await Mediator.Send(new GetVehiclesByKindIdQuery(kindId));
    }

    [HttpGet("vin{vinNumber}")]
    public async Task<ActionResult<VehicleByVinNumberDto?>> GetVehicleByVinNumber(string vinNumber)
    {
        return await Mediator.Send(new GetVehicleByVinNumberQuery(vinNumber));
    }

    [HttpGet("selfCustomer")]
    public async Task<ActionResult<List<VehicleByCustomerIdDto>>> GetVehiclesByCustomerId()
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetVehiclesByCustomerIdQuery(UserInfo.UserId));
    }
    [HttpPost]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<int>> Create(CreateVehicleCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }

        command.CustomerId = UserInfo.UserId;

        return await Mediator.Send(command);
    }
    [HttpPut("self{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> Update(int id, UpdateVehicleCommand command)
    {
        if (id != command.Id || UserInfo?.UserId != command.CustomerId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    [HttpDelete("self{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> Delete(int id)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }

        await Mediator.Send(new DeleteVehicleCommand(id, UserInfo.UserId));

        return NoContent();
    }
}
