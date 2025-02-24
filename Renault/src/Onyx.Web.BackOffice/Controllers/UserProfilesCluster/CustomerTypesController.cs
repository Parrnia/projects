using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Commands.CreateCustomerType;
using Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Commands.DeleteCustomerType;
using Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Commands.UpdateCustomerType;
using Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Queries;
using Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Queries.GetCustomerType;
using Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Queries.GetCustomerTypes;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Validators;
using Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Queries.Export.ExportExcelCustomerTypes;

namespace Onyx.Web.BackOffice.Controllers.UserProfilesCluster;


public class CustomerTypesController : ApiControllerBase
{
    
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<CustomerTypeDto>>> GetAllCustomerTypes()
    {
        return await Mediator.Send(new GetAllCustomerTypesQuery());
    }
    
    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<CustomerTypeDto?>> GetCustomerTypeById(int id)
    {
        return await Mediator.Send(new GetCustomerTypeByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelCustomerTypesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "CustomerTypes.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }


    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateCustomerTypeCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateCustomerTypeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.CustomerManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCustomerTypeCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeCustomerType([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeCustomerTypeCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueCustomerType")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueCustomerType([FromQuery] UniqueCustomerTypeValidator query)
    {
        return await Mediator.Send(query);
    }
}
