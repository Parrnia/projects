using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.CreateReturnOrder;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.SendReturnOrder;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrder.GetReturnOrderById;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrder.GetReturnOrderByNumber;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrders.GetReturnOrdersByCustomerId;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrders.GetReturnOrdersByOrderId;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrdersWithPagination.GetReturnOrdersByCustomerIdWithPagination;
using Onyx.Web.FrontOffice.Authorization;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;

namespace Onyx.Web.FrontOffice.Controllers.ReturnOrdersCluster;


public class ReturnOrdersController : ApiControllerBase
{
    [HttpGet("selfCustomer")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<List<ReturnOrderByCustomerIdDto>>> GetReturnOrdersByCustomerId()
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetReturnOrdersByCustomerIdQuery(UserInfo.UserId));
    }

    [HttpGet("self/{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<ReturnOrderByIdDto?>> GetReturnOrderById(int id)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetReturnOrderByIdQuery(id, UserInfo.UserId));
    }

    [HttpGet("selfOrderId/{orderId}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<List<ReturnOrderByOrderIdDto>>> GetReturnOrdersByOrderId(int orderId)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetReturnOrdersByOrderIdQuery(orderId, UserInfo.UserId));
    }

    [HttpGet("ByNumber")]
    public async Task<ActionResult<ReturnOrderByNumberDto?>> GetReturnOrderByNumber([FromQuery] GetReturnOrderByNumberQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<PaginatedList<ReturnOrderByCustomerIdWithPaginationDto>>> GetReturnOrdersByCustomerIdWithPagination([FromQuery] GetReturnOrdersByCustomerIdWithPaginationQuery query)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        query.CustomerId = UserInfo.UserId;

        return await Mediator.Send(query);
    }
    [HttpPost]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<int>> Create(CreateReturnOrderCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("send/{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> Send(int id, SendReturnOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
}
