using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Commands.CreateCustomerTicket;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Commands.DeleteCustomerTicket;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Commands.UpdateCustomerTicket;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice.GetCustomerTicket;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice.GetCustomerTickets;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice.GetCustomerTicketsWithPagination;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.Export.ExportExcelCustomerTickets;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.CustomerSupportCluster;


public class CustomerTicketsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredCustomerTicketDto>> GetCustomerTicketsWithPagination([FromQuery] GetCustomerTicketsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("customer{customerId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<CustomerTicketDto>>> GetCustomerTicketsByCustomerId(Guid customerId)
    {
        return await Mediator.Send(new GetCustomerTicketsByCustomerIdQuery(customerId));
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<CustomerTicketDto?>> GetCustomerTicketById(int id)
    {
        return await Mediator.Send(new GetCustomerTicketByIdQuery(id));
    }


    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelCustomerTicketsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "CustomerTickets.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateCustomerTicketCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateCustomerTicketCommand command)
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
        await Mediator.Send(new DeleteCustomerTicketCommand(id,null));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeCustomerTicket([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeCustomerTicketCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
