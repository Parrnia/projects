using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Commands.CreateAddress;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Commands.DeleteAddress;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Commands.UpdateAddress;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.BackOffice;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.BackOffice.GetAddress;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.BackOffice.GetAddresses;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Validators;
using Onyx.Application.Main.UserProfilesCluster.Addresses.Queries.Export.ExportExcelAddresses;

namespace Onyx.Web.BackOffice.Controllers.UserProfilesCluster;


public class AddressesController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AddressDto>>> GetAllAddresses()
    {
        return await Mediator.Send(new GetAllAddressesQuery());
    }
    
    [HttpGet("customer{customerId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AddressDto>>> GetAddressesByCustomerId(Guid customerId)
    {
        return await Mediator.Send(new GetAddressesByCustomerIdQuery(customerId));
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<AddressDto?>> GetAddressById(int id)
    {
        return await Mediator.Send(new GetAddressByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelAddressesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Addresses.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }


    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateAddressCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateAddressCommand command)
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
        await Mediator.Send(new DeleteAddressCommand(id,null));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeAddress([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeAddressCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
    //validators
    [HttpGet("isUniqueTitle")]
    public async Task<ActionResult<bool>> IsUniqueAddressTitle([FromQuery] UniqueAddressTitleValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniquePostcode")]
    public async Task<ActionResult<bool>> IsUniqueAddressPostcode([FromQuery] UniqueAddressPostcodeValidator query)
    {
        return await Mediator.Send(query);
    }
}
