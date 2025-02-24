using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Commands.CreateCustomerTicket;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Commands.DeleteCustomerTicket;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice.GetCustomerTicket;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice.GetCustomerTickets;
using Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice.GetCustomerTicketsWithPagination;
using Onyx.Web.FrontOffice.Authorization;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;

namespace Onyx.Web.FrontOffice.Controllers.CustomerSupportCluster;


public class CustomerTicketsController : ApiControllerBase
{
    [HttpGet]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<FilteredCustomerTicketDto>> GetCustomerTicketsWithPagination([FromQuery] GetCustomerTicketsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("customer{customerId}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<List<CustomerTicketDto>>> GetCustomerTicketsByCustomerId(Guid customerId)
    {
        return await Mediator.Send(new GetCustomerTicketsByCustomerIdQuery(customerId));
    }

    [HttpGet("{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<CustomerTicketDto?>> GetCustomerTicketById(int id)
    {
        return await Mediator.Send(new GetCustomerTicketByIdQuery(id));
    }
    
    [HttpPost]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<int>> Create(CreateCustomerTicketCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        command.CustomerId = UserInfo.UserId;
        command.CustomerPhoneNumber = UserInfo.PhoneNumber;
        command.CustomerName = UserInfo.FirstName + " " + UserInfo.LastName;
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCustomerTicketCommand(id,null));

        return NoContent();
    }

    [HttpDelete]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<IActionResult> DeleteRangeCustomerTicket([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeCustomerTicketCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
