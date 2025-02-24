using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.UserProfilesCluster.Customers.Commands.AddVehicleToCustomer;
using Onyx.Application.Main.UserProfilesCluster.Customers.Commands.CreateCustomer;
using Onyx.Application.Main.UserProfilesCluster.Customers.Commands.RemoveVehicleFromCustomer;
using Onyx.Application.Main.UserProfilesCluster.Customers.Commands.UpdateCustomer;
using Onyx.Application.Main.UserProfilesCluster.Customers.Queries.FrontOffice;
using Onyx.Application.Main.UserProfilesCluster.Customers.Queries.FrontOffice.GetCustomer;
using Onyx.Web.FrontOffice.Authorization;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;


namespace Onyx.Web.FrontOffice.Controllers.UserProfilesCluster;

public class CustomersController : ApiControllerBase
{
    [HttpGet]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<CustomerDto?>> GetCustomerById()
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetCustomerByIdQuery(UserInfo.UserId));
    }

    [HttpPost]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<Guid>> Create(CreateCustomerCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }

        command.Id = UserInfo.UserId;

        return await Mediator.Send(command);
    }

    [HttpPut("{urlId}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> Update(Guid urlId, UpdateCustomerCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }

        command.Id = UserInfo.UserId;

        if (urlId != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("addVehicle")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> AddVehicleToCustomer(AddVehicleToCustomerCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }

        command.CustomerId = UserInfo.UserId;

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("removeVehicle")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> RemoveVehicleFromCustomer(int vehicleId)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        await Mediator.Send(new RemoveVehicleFromCustomerCommand(UserInfo.UserId, vehicleId));

        return NoContent();
    }
}
