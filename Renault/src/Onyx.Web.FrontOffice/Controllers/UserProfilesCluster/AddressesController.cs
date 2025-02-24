using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Commands.CreateAddress;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Commands.DeleteAddress;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Commands.UpdateAddress;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.FrontOffice;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.FrontOffice.GetAddress.GetAddressById;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.FrontOffice.GetAddress.GetDefaultAddress;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.FrontOffice.GetAddresses.GetAddressesByCustomerId;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Validators;
using Onyx.Web.FrontOffice.Authorization;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared;

namespace Onyx.Web.FrontOffice.Controllers.UserProfilesCluster;


public class AddressesController : ApiControllerBase
{
    [HttpGet("selfCustomer")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<List<AddressDto>>> GetAddressesByCustomerId()
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetAddressesByCustomerIdQuery(UserInfo.UserId));
    }
    [HttpGet("{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<AddressDto?>> GetAddressById(int id)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetAddressByIdQuery(id, UserInfo.UserId));
    }
    [HttpGet("selfDefault")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<AddressDto?>> GetDefaultAddress()
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetDefaultAddressQuery(UserInfo.UserId));
    }
    [HttpPost]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<int>> Create(CreateAddressCommand command)
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
    public async Task<ActionResult> SelfUpdate(int id, UpdateAddressCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }
        command.CustomerId = UserInfo.UserId;

        await Mediator.Send(command);

        return NoContent();
    }
    [HttpDelete("self{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> SelfDelete(int id)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        await Mediator.Send(new DeleteAddressCommand(id, UserInfo?.UserId));

        return NoContent();
    }

    //validators
    [HttpGet("isUniqueTitle")]
    public async Task<ActionResult<bool>> IsUniqueAddressTitle([FromQuery] UniqueAddressTitleValidator query)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        query.CustomerId = UserInfo.UserId;

        return await Mediator.Send(query);
    }
    [HttpGet("isUniquePostcode")]
    public async Task<ActionResult<bool>> IsUniqueAddressPostcode([FromQuery] UniqueAddressPostcodeValidator query)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        query.CustomerId = UserInfo.UserId;

        return await Mediator.Send(query);
    }
}
