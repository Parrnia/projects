using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Commands.CreateProductAttributeOption;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Commands.DeleteProductAttributeOption;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Commands.UpdateProductAttributeOption;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Queries.BackOffice;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Queries.Export.ExportExcelProductAttributeOptions;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster.ProductOptionsCluster;


public class ProductAttributeOptionsController : ApiControllerBase
{
    
    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductAttributeOptionCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateProductAttributeOptionCommand command)
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
        await Mediator.Send(new DeleteProductAttributeOptionCommand(id));

        return NoContent();
    }
    
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductAttributeOptionCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("product{productId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductAttributeOptionDto>>> GetAllProductAttributeOptionByProductId(int productId)
    {
        return await Mediator.Send(new GetAllProductAttributeOptionsByProductIdQuery(productId));
    }



    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductAttributeOptionsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductAttributeOptions.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

}
