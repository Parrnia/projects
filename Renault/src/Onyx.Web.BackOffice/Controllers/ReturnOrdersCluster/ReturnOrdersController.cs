using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.OrdersCluster.OrderItemOptions.Queries.BackOffice.GetOrderItemOptions;
using Onyx.Application.Main.OrdersCluster.OrderItems.Queries.BackOffice.GetOrderItems;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrderStates;
using Onyx.Application.Main.OrdersCluster.OrderTotals.Queries.BackOffice.GetOrderTotals;
using Onyx.Application.Main.ReturnOrdersCluster.OptionValues.Queries.BackOffice;
using Onyx.Application.Main.ReturnOrdersCluster.OptionValues.Queries.BackOffice.GetOptionValues;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Queries.BackOffice;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Queries.BackOffice.GetReturnOrderItemDocuments;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemGroups.Queries.BackOffice;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemGroups.Queries.BackOffice.GetReturnOrderItemGroups;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Queries.BackOffice;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Queries.BackOffice.GetReturnOrderItems;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.AcceptReturnOrder;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.CancelReturnOrder;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.CompleteReturnOrder;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ConfirmAllReturnOrder;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ConfirmSomeReturnOrder;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.CreateReturnOrder;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReceiveReturnOrder;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.RefundReturnOrderCost;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.RejectReturnOrder;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.SendReturnOrder;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.UpdateReturnOrder;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrder;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrders;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrderStates;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrdersWithPagination;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelAcceptedReturnOrders;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelAllConfirmedReturnOrders;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelCanceledReturnOrders;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelCompletedReturnOrders;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelCostRefundedReturnOrders;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelReceivedReturnOrders;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelRegisteredReturnOrders;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelRejectedReturnOrders;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelReturnOrders;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelSentReturnOrders;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelSomeConfirmedReturnOrders;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.BackOffice;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.BackOffice.GetReturnOrderTotal.GetSendReturnOrderTotal;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotals.Queries.BackOffice.GetReturnOrderTotals;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.ReturnOrdersCluster;


public class ReturnOrdersController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderDto>>> GetAllReturnOrders()
    {
        return await Mediator.Send(new GetAllReturnOrdersQuery());
    }

    [HttpGet("count")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> GetReturnOrdersCount()
    {
        return await Mediator.Send(new GetReturnOrdersCountQuery());
    }

    [HttpGet]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredReturnOrderDto>> GetReturnOrdersWithPagination([FromQuery] GetReturnOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("accepted")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredReturnOrderDto>> GetAcceptedReturnOrders([FromQuery] GetAcceptedReturnOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("allConfirmed")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredReturnOrderDto>> GetAllConfirmedReturnOrders([FromQuery] GetAllConfirmedReturnOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("canceled")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredReturnOrderDto>> GetCanceledReturnOrders([FromQuery] GetCanceledReturnOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("completed")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredReturnOrderDto>> GetCompletedReturnOrders([FromQuery] GetCompletedReturnOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("costRefunded")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredReturnOrderDto>> GetCostRefundedReturnOrders([FromQuery] GetCostRefundedReturnOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("received")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredReturnOrderDto>> GetReceivedReturnOrders([FromQuery] GetReceivedReturnOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("registered")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredReturnOrderDto>> GetRegisteredReturnOrders([FromQuery] GetRegisteredReturnOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("rejected")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredReturnOrderDto>> GetRejectedReturnOrders([FromQuery] GetRejectedReturnOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("sent")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredReturnOrderDto>> GetSentReturnOrders([FromQuery] GetSentReturnOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("someConfirmed")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredReturnOrderDto>> GetSomeConfirmedReturnOrders([FromQuery] GetSomeConfirmedReturnOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("customer/{customerId}")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderDto>>> GetReturnOrdersByCustomerId(Guid customerId)
    {
        return await Mediator.Send(new GetReturnOrdersByCustomerIdQuery(customerId));
    }

    [HttpGet("product/{productId}")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderDto>>> GetReturnOrdersByProductId(int productId)
    {
        return await Mediator.Send(new GetReturnOrdersByProductIdQuery(productId));
    }

    [HttpGet("info/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ReturnOrderInfoDto?>> GetReturnOrderInfo(int id)
    {
        return await Mediator.Send(new GetReturnOrderInfoQuery(id));
    }

    [HttpGet("returnOrderItem/{returnOrderGroupId}")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderItemDto>>> GetReturnOrderItemsByReturnOrderItemGroupId(int returnOrderGroupId)
    {
        return await Mediator.Send(new GetReturnOrderItemsByReturnOrderItemGroupIdQuery(returnOrderGroupId));
    }

    [HttpGet("returnOrderTotal/{returnOrderId}")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderTotalDto>>> GetReturnOrderTotalsByReturnOrderId(int returnOrderId)
    {
        return await Mediator.Send(new GetReturnOrderTotalsByReturnOrderIdQuery(returnOrderId));
    }

    [HttpGet("sendReturnOrderTotal/{returnOrderId}")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<SendReturnOrderTotalDto?>> GetSendReturnOrderTotalByReturnOrderId(int returnOrderId)
    {
        return await Mediator.Send(new GetSendReturnOrderTotalByReturnOrderIdQuery(returnOrderId));
    }

    [HttpGet("returnOrderItemGroup/{returnOrderId}")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderItemGroupDto>>> GetReturnOrderItemGroupsByReturnOrderId(int returnOrderId)
    {
        return await Mediator.Send(new GetReturnOrderItemGroupsByReturnOrderIdQuery(returnOrderId));
    }

    [HttpGet("returnOrderItemGroupOption/{returnOrderItemGroupId}")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderItemGroupProductAttributeOptionValueDto>>> GetOptionValuesByReturnOrderItemGroupId(int returnOrderItemGroupId)
    {
        return await Mediator.Send(new GetOptionValuesByReturnOrderItemGroupIdQuery(returnOrderItemGroupId));
    }

    [HttpGet("returnOrderItemDocument/{returnOrderItemId}")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderItemDocumentDto>>> GetReturnOrderItemDocumentsByReturnOrderItemId(int returnOrderItemId)
    {
        return await Mediator.Send(new GetReturnOrderItemDocumentsByReturnOrderItemIdQuery(returnOrderItemId));
    }

    [HttpGet("returnOrderState/{returnOrderId}")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ReturnOrderStateBaseDto>>> GetReturnOrderStatesByReturnOrderId(int returnOrderId)
    {
        return await Mediator.Send(new GetReturnOrderStatesByReturnOrderIdQuery(returnOrderId));
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ReturnOrderDto?>> GetReturnOrderById(int id)
    {
        return await Mediator.Send(new GetReturnOrderByIdQuery(id));
    }


    [HttpGet("currentReturnOrderState/{returnOrderId}")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ReturnOrderStateBaseDto?>> GetCurrentReturnOrderStateByReturnOrderId(int returnOrderId)
    {
        return await Mediator.Send(new GetCurrentReturnOrderStateByReturnOrderIdQuery(returnOrderId));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelReturnOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ReturnOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelAccepted")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelAcceptedQuery([FromQuery] ExportExcelAcceptedReturnOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "AcceptedReturnOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelAllConfirmed")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelAllConfirmedQuery([FromQuery] ExportExcelAllConfirmedReturnOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "AllConfirmedReturnOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelCanceled")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelCanceledQuery([FromQuery] ExportExcelCanceledReturnOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "CanceledReturnOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelCompleted")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelCompletedQuery([FromQuery] ExportExcelCompletedReturnOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "CompletedReturnOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelCostRefunded")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelCostRefundedQuery([FromQuery] ExportExcelCostRefundedReturnOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "CostRefundedReturnOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelReceived")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelReceivedQuery([FromQuery] ExportExcelReceivedReturnOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ReceivedReturnOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelRegistered")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelRegisteredQuery([FromQuery] ExportExcelRegisteredReturnOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "RegisteredReturnOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelRejected")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelRejectedQuery([FromQuery] ExportExcelRejectedReturnOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "RejectedReturnOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelSent")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelSentQuery([FromQuery] ExportExcelSentReturnOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "SentReturnOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelSomeConfirmed")]
    [CheckPermission(Roles.Employee, UserPermissions.ReturnOrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelSomeConfirmedQuery([FromQuery] ExportExcelSomeConfirmedReturnOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "SomeConfirmedReturnOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.CreateReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateReturnOrderCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("accept/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.AcceptOrRejectReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Accept(int id, AcceptReturnOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("cancel/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Cancel(int id, CancelReturnOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("complete/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Complete(int id, CompleteReturnOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("confirmAll/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> ConfirmAll(int id, ConfirmAllReturnOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("confirmSome/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> ConfirmSome(int id, ConfirmSomeReturnOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("receive/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ReceiveReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Receive(int id, ReceiveReturnOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("refundCost/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.RefundCostReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> RefundCost(int id, RefundReturnOrderCostCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("reject/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.AcceptOrRejectReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Reject(int id, RejectReturnOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("send/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Send(int id, SendReturnOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("update/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmReturnOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateReturnOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
}
