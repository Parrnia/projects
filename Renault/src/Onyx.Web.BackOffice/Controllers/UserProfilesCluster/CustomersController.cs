using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.UserProfilesCluster.Customers.Commands.AddVehicleToCustomer;
using Onyx.Application.Main.UserProfilesCluster.Customers.Commands.CreateCustomer;
using Onyx.Application.Main.UserProfilesCluster.Customers.Commands.RemoveVehicleFromCustomer;
using Onyx.Application.Main.UserProfilesCluster.Customers.Commands.SetCustomerCredit;
using Onyx.Application.Main.UserProfilesCluster.Customers.Commands.SetCustomerMaxCredit;
using Onyx.Application.Main.UserProfilesCluster.Customers.Commands.UpdateCustomer;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.UserProfilesCluster.Customers.Queries.BackOffice;
using Onyx.Application.Main.UserProfilesCluster.Customers.Queries.BackOffice.GetCustomer;
using Onyx.Application.Main.UserProfilesCluster.Customers.Queries.BackOffice.GetCustomers;

namespace Onyx.Web.BackOffice.Controllers.UserProfilesCluster;


public class CustomersController : ApiControllerBase
{

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<CustomerDto>>> GetAllCustomers()
    {
        return await Mediator.Send(new GetAllCustomersQuery());
    }

    [HttpGet("byIds")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<CustomerDto>>> GetCustomersByIds([FromQuery] GetCustomersByIdsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<CustomerDto?>> GetCustomerById(Guid id)
    {
        return await Mediator.Send(new GetCustomerByIdQuery(id));
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<Guid>> Create(CreateCustomerCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(Guid urlId, UpdateCustomerCommand command)
    {
        if (urlId != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("setCredit")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerCreditManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> SetCustomerCredit(SetCustomerCreditCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }

        command.ModifierUserId = UserInfo.UserId;
        command.ModifierUserName = UserInfo.FirstName + " " + UserInfo.LastName;

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("setMaxCredit")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerCreditManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> SetMaxCustomerCredit(SetCustomerMaxCreditCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }

        command.ModifierUserId = UserInfo.UserId;
        command.ModifierUserName = UserInfo.FirstName + " " + UserInfo.LastName;

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("addVehicle")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
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
    [CheckPermission(Roles.Employee, UserPermissions.CustomerManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
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
