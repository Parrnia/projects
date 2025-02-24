using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.OrdersCluster.OrderItems.Queries.BackOffice.GetOrderItems;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.CancelOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.ConfirmOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.PrepareOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.RefundOrderCost;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.ShipOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.UnConfirmOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.UpdateOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrders;
using Onyx.Application.Main.OrdersCluster.OrderTotals.Queries.BackOffice.GetOrderTotals;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.OrdersCluster.OrderItems.Queries.BackOffice;
using Onyx.Application.Main.OrdersCluster.OrderTotals.Queries.BackOffice;
using Onyx.Application.Main.OrdersCluster.OrderItemOptions.Queries.BackOffice;
using Onyx.Application.Main.OrdersCluster.OrderItemOptions.Queries.BackOffice.GetOrderItemOptions;
using Onyx.Application.Main.OrdersCluster.OrderPayments.BackOffice.Queries;
using Onyx.Application.Main.OrdersCluster.OrderPayments.BackOffice.Queries.GetOrderPayments;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.CompleteOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrderStates;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrdersWithPagination;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.ConfirmOrderPayment;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.DeleteOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.UnConfirmOrderPayment;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelOrders;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelCanceledOrders;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelCompletedOrders;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelConfirmedOrders;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelCostRefundedOrders;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelPaymentConfirmedOrders;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelPaymentUnconfirmedOrders;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelPreparedOrders;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelRegisteredOrders;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelShippedOrders;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.Export.ExportExcelUnconfirmedOrders;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.PayOrder.OnlinePay;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.PayOrder.CreditPay;

namespace Onyx.Web.BackOffice.Controllers.OrdersCluster;


public class OrdersController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<OrderDto>>> GetAllOrders()
    {
        return await Mediator.Send(new GetAllOrdersQuery());
    }

    [HttpGet("count")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> GetOrdersCount()
    {
        return await Mediator.Send(new GetOrdersCountQuery());
    }

    [HttpGet]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredOrderDto>> GetOrdersWithPagination([FromQuery] GetOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("registered")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredOrderDto>> GetRegisteredOrders([FromQuery] GetRegisteredOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("confirmed")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredOrderDto>> GetConfirmedOrders([FromQuery] GetConfirmedOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("paymentConfirmed")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredOrderDto>> GetPaymentConfirmedOrders([FromQuery] GetPaymentConfirmedOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("prepared")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredOrderDto>> GetPreparedOrders([FromQuery] GetPreparedOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("shipped")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredOrderDto>> GetShippedOrders([FromQuery] GetShippedOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("unconfirmed")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredOrderDto>> GetUnconfirmedOrders([FromQuery] GetUnconfirmedOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("paymentUnconfirmed")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredOrderDto>> GetPaymentUnconfirmedOrders([FromQuery] GetPaymentUnconfirmedOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("canceled")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredOrderDto>> GetCanceledOrders([FromQuery] GetCanceledOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("costRefunded")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredOrderDto>> GetCostRefundedOrders([FromQuery] GetCostRefundedOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("completed")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredOrderDto>> GetCompletedOrders([FromQuery] GetCompletedOrdersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("customer/{customerId}")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<OrderDto>>> GetOrdersByCustomerId(Guid customerId)
    {
        return await Mediator.Send(new GetOrdersByCustomerIdQuery(customerId));
    }

    [HttpGet("product/{productId}")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<OrderDto>>> GetOrdersByProductId(int productId)
    {
        return await Mediator.Send(new GetOrdersByProductIdQuery(productId));
    }

    [HttpGet("info/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<OrderInfoDto>> GetOrderInfo(int id)
    {
        return await Mediator.Send(new GetOrderInfoQuery(id));
    }

    [HttpGet("orderItem/{orderId}")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<OrderItemDto>>> GetOrderItemsByOrderId(int orderId)
    {
        return await Mediator.Send(new GetOrderItemsByOrderIdQuery(orderId));
    }

    [HttpGet("orderTotal/{orderId}")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<OrderTotalDto>>> GetOrderTotalsByOrderId(int orderId)
    {
        return await Mediator.Send(new GetOrderTotalsByOrderIdQuery(orderId));
    }

    [HttpGet("orderPayment/{orderId}")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<OrderPaymentDto>>> GetOrderPaymentsByOrderId(int orderId)
    {
        return await Mediator.Send(new GetOrderPaymentsByOrderIdQuery(orderId));
    }

    [HttpGet("orderItemOption/{orderItemId}")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<OrderItemOptionDto>>> GetOrderItemOptionsByOrderItemId(int orderItemId)
    {
        return await Mediator.Send(new GetOrderItemOptionsByOrderItemIdQuery(orderItemId));
    }

    [HttpGet("orderState/{orderId}")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<OrderStateBaseDto>>> GetOrderStatesByOrderId(int orderId)
    {
        return await Mediator.Send(new GetOrderStatesByOrderIdQuery(orderId));
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<OrderDto?>> GetOrderById(int id)
    {
        return await Mediator.Send(new GetOrderByIdQuery(id));
    }

    [HttpGet("currentOrderState/{orderId}")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<OrderStateBaseDto?>> GetCurrentOrderStateByOrderId(int orderId)
    {
        return await Mediator.Send(new GetCurrentOrderStateByOrderIdQuery(orderId));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Orders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelCanceled")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelCanceledQuery([FromQuery] ExportExcelCanceledOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "CanceledOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelCompleted")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelCompletedQuery([FromQuery] ExportExcelCompletedOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "CompletedOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelConfirmed")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelConfirmedQuery([FromQuery] ExportExcelConfirmedOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ConfirmedOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelCostRefunded")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelCostRefundedQuery([FromQuery] ExportExcelCostRefundedOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "CostRefundedOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelPaymentConfirmed")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelPaymentConfirmedQuery([FromQuery] ExportExcelPaymentConfirmedOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "PaymentConfirmedOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelPaymentUnconfirmed")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelPaymentUnconfirmedQuery([FromQuery] ExportExcelPaymentUnconfirmedOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "PaymentUnconfirmedOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelPrepared")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelPreparedQuery([FromQuery] ExportExcelPreparedOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ExcelPreparedOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelRegistered")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelRegisteredQuery([FromQuery] ExportExcelRegisteredOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "RegisteredOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelShipped")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelShippedQuery([FromQuery] ExportExcelShippedOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ShippedOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("exportExcelUnconfirmed")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelUnconfirmedQuery([FromQuery] ExportExcelUnconfirmedOrdersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "UnconfirmedOrders.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.UpdateOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("cancel/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.CancelOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Cancel(int id, CancelOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("confirm/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmOrUnconfirmOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Confirm(int id, ConfirmOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("confirmPayment/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmOrUnconfirmOrderPayment)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> ConfirmPayment(int id, ConfirmOrderPaymentCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("prepare/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.PrepareOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Prepare(int id, PrepareOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("ship/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ShipOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Ship(int id, ShipOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("unConfirm/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmOrUnconfirmOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> UnConfirm(int id, UnConfirmOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("unConfirmPayment/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.ConfirmOrUnconfirmOrderPayment)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> UnConfirmPayment(int id, UnConfirmOrderPaymentCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("refundCost/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.RefundOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> RefundCost(int id, RefundOrderCostCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("complete/{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.OrderManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Complete(int id, CompleteOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }



    [HttpDelete("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.DeleteOrder)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteOrderCommand(id));

        return NoContent();
    }
}
