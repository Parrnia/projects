using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrder.GetOrderById;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrders.GetOrdersByCustomerId;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrdersWithPagination.GetOrdersByCustomerIdWithPagination;
using Onyx.Domain.Enums;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetInvoice;
using Onyx.Web.FrontOffice.Authorization;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrder.GetOrderByNumber;
using Microsoft.AspNetCore.Authorization;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.CreateOrder.CreateOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrder.GetOrderForReturnById;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.PayOrder.OnlinePay;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.PayOrder.CreditPay;
using Onyx.Application.Services.PaymentServices.Responses;
using OnyxAuth.Shared.Permissions;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.VerifyPayment;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.CompleteOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.SelfDeleteOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.UpdateOrder;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.UpdateOrderPriceAndCount;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Microsoft.Net.Http.Headers;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Onyx.Application.Main.OrdersCluster.Orders.Commands.PayOrder.CreditOnlinePay;

namespace Onyx.Web.FrontOffice.Controllers.OrdersCluster;

public class OrdersController : ApiControllerBase
{
    [HttpGet("selfCustomer")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<List<OrderByCustomerIdDto>>> GetOrdersByCustomerId()
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetOrdersByCustomerIdQuery(UserInfo.UserId));
    }

    [HttpGet("self/{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<OrderByIdDto?>> GetOrderById(int id)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetOrderByIdQuery(id, UserInfo.UserId));
    }

    [HttpGet("selfForReturn/{orderId}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<OrderForReturnByIdDto?>> GetOrderForReturnById(int orderId)

    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetOrderForReturnByIdQuery(orderId, UserInfo.UserId));
    }

    [HttpGet("ByNumber")]
    public async Task<ActionResult<OrderByNumberDto?>> GetOrderByNumber([FromQuery] GetOrderByNumberQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<PaginatedList<OrderByCustomerIdWithPaginationDto>>> GetOrdersByCustomerIdWithPagination([FromQuery] GetOrdersByCustomerIdWithPaginationQuery query)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        query.CustomerId = UserInfo.UserId;

        return await Mediator.Send(query);
    }

    [HttpGet("selfInvoice/{orderId}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<OrderInvoiceDto?>> GetInvoice(int orderId)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        return await Mediator.Send(new GetOrderInvoiceQuery(orderId, UserInfo?.UserId));
    }

    [HttpPut("update/{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> Update(int id, UpdateOrderPriceAndCountCommand command)
    {
        if (id != command.OrderId || UserInfo == null)
        {
            return BadRequest();
        }
        command.CustomerId = UserInfo.UserId;

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult> Delete(int id, SelfDeleteOrderCommand command)
    {
        if (id != command.Id || UserInfo == null)
        {
            return BadRequest();
        }
        command.CustomerId = UserInfo.UserId;

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPost]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<int>> CreateOrder(CreateOrderCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        command.CustomerId = UserInfo.UserId;
        command.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        command.PersonType = (PersonType)UserInfo.PersonType;
        command.FirstName = UserInfo.FirstName;
        command.LastName = UserInfo.LastName;
        command.CustomerId = UserInfo.UserId;
        command.PhoneNumber = UserInfo.PhoneNumber;
        command.NationalCode = UserInfo.NationalCode;


        return await Mediator.Send(command);
    }

    [HttpPost("creditOnlinePay")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<StartPaymentResult>> CreateCreditOnlineOrderPay(CreateCreditOnlineOrderPayCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        command.CustomerId = UserInfo.UserId;

        return await Mediator.Send(command);
    }

    [HttpPost("onlinePay")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<StartPaymentResult>> CreateOnlineOrderPay(CreateOnlineOrderPayCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        command.CustomerId = UserInfo.UserId;

        return await Mediator.Send(command);
    }

    [HttpPost("creditPay")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<int>> CreateCreditOrderPay(CreateCreditOrderPayCommand command)
    {
        if (UserInfo == null)
        {
            return BadRequest();
        }
        command.CustomerId = UserInfo.UserId;

        return await Mediator.Send(command);
    }


    [HttpPost("callback")]
    [AllowAnonymous]
    public async Task<IActionResult> Callback(int paymentId)
    {
        var command = new VerifyPaymentCommand() { PaymentId = paymentId };
        var result = await Mediator.Send(command);

        var htmlResponse = string.Empty;

        if (result.IsSuccess)
        {
            htmlResponse = $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Payment Successful</title>
              <style>
        /* Resetting margins and paddings */
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}

        /* Full page background */
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #ffe500, #ffe500); /* gradient background */
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            color: #28a745;
            text-align: center;
            direction: rtl;
        
        }}

        /* Container with rounded corners */
        .container {{
            background: rgba(255, 255, 255, 0.9); /* semi-transparent white */
            padding: 15px;
            border-radius: 12px;
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
            min-width: 400px;
            max-width: 600px;
            height: auto;
        }}

        /* Status icon styling */
        .status-icon {{
            font-size: 4em;
            color: #28a745;
            margin-bottom: 15px;
        }}

        /* Message text */
        .message {{
            font-size: 2em;
            margin-bottom: 15px;
            font-weight: bold;
        }}

        /* Transaction details */
        .details {{
            font-size: 1em;
            margin-bottom: 30px;
            color: #666;
        }}

        .details strong {{
            color: #444;
        }}
        /* Button styling */
        .button {{
            display: flex;
            justify-content: center;
            align-items: center;
           padding: 8px;
            background: #28a745;
            color: white;
            text-decoration: none;
            cursor: pointer;
            border-radius: 5px;
            font-size: 1.1em;
            transition: background-color 0.3s;
        }}

        /* Hover effect for button */
        .button:hover {{
            background-color: #28a746b9;
        }}

        /* Responsive design for smaller screens */
        @media (max-width: 600px) {{
            .container {{
                padding: 20px;
            }}

            .status-icon {{
                font-size: 4em;
            }}

            .message {{
                font-size: 1.5em;
            }}

            .button {{
                padding: 12px 25px;
            }}
        }}
        @media (max-width: 320px) {{

            .container {{
                padding: 20px;
                width: 85%;
            }}

            .status-icon {{
                font-size: 4em;
            }}

            .message {{
                font-size: 1.5em;
            }}

            .button {{
                padding: 12px 25px;
            }}
        }}
        .table {{
            margin-top: 15px;
        }}
        .table .table-row{{
            display: flex;
            justify-content: space-between;
            margin: 5px 0;
            padding: 4px;

        }}
        .table-row > span {{
            font-size: 14px;
        }}
    </style>
            </head>
            <body>
                <div class=""container"">
        <div class=""status-icon"">✔</div>
        <div class=""message"">پرداخت موفق</div>
        <div class=""details"">
            اطلاعات پرداخت شما <br>
            <div class=""table"">
                <div class=""table-row"">
                    <span>
                        مبلغ
                    </span>
                    <strong> {result.Amount} ریال</strong>
                 
                </div>
                <div class=""table-row"">
                    <span>
                        شماره پیگیری شبکه بانکی:
                    </span>
                    <strong> {result.Rrn}</strong>
                </div>
                <div class=""table-row"">
                    <span>
                        شماره تراکنش سوییچ:
                                    </span>
                    <strong> {result.PayGateTranId}</strong>
                </div>
                <div class=""table-row"">
                    <span>
                        شماره پیگیری: 
                                    </span>
                    <strong> {result.RefId}</strong>
                </div>
            </div>
        </div>
        <a href=""{result.SiteUrl}/account/orders/{result.OrderId}"" class=""button"">رفتن به صفحه سفارش</a>
    </div>
            </body>
            </html>
            ";
        }

        else
        {
            switch (result.ErrorMessage)
            {
                case "No record found. (1034)":
                    result.ErrorMessage = "انصراف مشتری";
                    break;
            }
            htmlResponse = $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Payment Failed</title>
                <style>
                    /* Resetting margins and paddings */
                    * {{
                        margin: 0;
                        padding: 0;
                        box-sizing: border-box;
                    }}

                    /* Full page background */
                    body {{
                        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #ffe500, #ffe500); /* gradient background */
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            color: #cd2c2c;
            text-align: center;
                        direction: rtl;
                    }}

                    /* Container with rounded corners */
                    .container {{
                       background: rgba(255, 255, 255, 0.9); /* semi-transparent white */
                       padding: 15px;
            border-radius: 12px;
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
            min-width: 400px;
            max-width: 600px;
            height: auto;
                    }}

                    /* Status icon styling */
                    .status-icon {{
                        font-size: 4em;
                        color: #dc3545;
                        margin-bottom: 15px;
                    }}

                    /* Message text */
                    .message {{
                        font-size: 2em;
                        margin-bottom: 15px;
                        font-weight: bold;
                    }}

                    /* Error details */
                    .details {{
                        font-size: 1em;
                        margin-bottom: 30px;
                        color: #666;
                    }}

                    .details strong {{
                        color: #444;
                    }}

                    /* Button styling */
                    .button {{
                         background-color: #696969;
                        color: white;
                        text-decoration: none;
                        border-radius: 5px;
                        font-size: 1.1em;
                        transition: background-color 0.3s;

                        display: flex;
                        justify-content: center;
                 align-items: center;
           padding: 8px;
            cursor: pointer;
                    }}

                    /* Hover effect for button */
                    .button:hover {{
                        background-color: #515151b2;
                    }}

                    /* Responsive design for smaller screens */
                    @media (max-width: 600px) {{
                        .container {{
                            padding: 20px;
                        }}

                        .status-icon {{
                            font-size: 4em;
                        }}

                        .message {{
                            font-size: 1.5em;
                        }}

                        .button {{
                            padding: 12px 25px;
                        }}
                    }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <div class=""status-icon"">✘</div>
                    <div class=""message"">پرداخت ناموفق</div>
                    <div class=""details"">
                        متاسفانه پرداخت شما ناموفق بوده است. <br>
                        علت: <strong>{result.ErrorMessage}</strong>
                    </div>
                    <a href=""{result.SiteUrl}/account/orders/{result.OrderId}"" class=""button"">تلاش دوباره</a>
                </div>
            </body>
            </html>
            ";
        }

        return new ContentResult()
        {
            ContentType = "text/html",
            Content = htmlResponse,
            StatusCode = 200,
        };

    }

}
