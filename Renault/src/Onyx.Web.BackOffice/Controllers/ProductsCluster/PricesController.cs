using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.Prices.Commands.CreatePrice;
using Onyx.Application.Main.ProductsCluster.Prices.Commands.DeletePrice;
using Onyx.Application.Main.ProductsCluster.Prices.Commands.UpdatePrice;
using Onyx.Application.Main.ProductsCluster.Prices.Queries.BackOffice;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.Prices.Queries.Export.ExportExcelPrices;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;

public class PricesController : ApiControllerBase
{


    
    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create( CreatePriceCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdatePriceCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }


    [HttpGet("option{optionId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<PriceDto>>> GetAllProductPricesByOptionId(int optionId)
    {
        return await Mediator.Send(new GetAllPricesByOptionIdQuery(optionId));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelPricesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Prices.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }


    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangePricesCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
