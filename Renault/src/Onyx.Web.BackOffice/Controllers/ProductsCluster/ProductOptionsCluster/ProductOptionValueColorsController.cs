using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Commands.DeleteProductOptionColor;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionColors.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Commands.CreateProductOptionValueColor;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Commands.DeleteProductOptionValueColor;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Commands.UpdateProductOptionValueColor;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Queries.BackOffice;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Queries.Export.ExportExcelProductOptionValueColors;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster.ProductOptionsCluster;


public class ProductOptionValueColorsController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductOptionValueColorDto>>> GetAllProductOptionValueColors()
    {
        return await Mediator.Send(new GetAllProductOptionValueColorsQuery());
    }

    [HttpGet("optionColors{colorId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductOptionValueColorDto>>> GetAllProductOptionValueByColorId(int colorId)
    {
        return await Mediator.Send(new GetAllProductOptionValueColorsByColorIdQuery(colorId));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductOptionValueColorsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductOptionValueColors.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductOptionValueColorCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateProductOptionValueColorCommand command)
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
        await Mediator.Send(new DeleteProductOptionColorCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductOptionValueCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}


