using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Commands.CreateProductAttribute;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Commands.DeleteProductAttribute;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Commands.DeleteRangeProductAttributes;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Commands.UpdateProductAttribute;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.BackOffice.GetProductAttribute;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.BackOffice.GetProductAttributes;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Queries.Export.ExportExcelProductAttributes;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster.ProductAttributesCluster;


public class ProductAttributesController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductAttributeDto>>> GetAllProductAttributes()
    {
        return await Mediator.Send(new GetAllProductAttributesQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProductAttributeDto?>> GetProductAttributeById(int id)
    {
        return await Mediator.Send(new GetProductAttributeByIdQuery(id));
    }

    [HttpGet("product{productId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductAttributeDto>>> GetAllProductAttributesByProductId(int productId)
    {
        return await Mediator.Send(new GetProductAttributeByProductIdQuery(productId));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductAttributesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductAttributes.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductAttributeCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateProductAttributeCommand command)
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
        await Mediator.Send(new DeleteProductAttributeCommand(id));

        return NoContent();
    }
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductAttributesCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

}
