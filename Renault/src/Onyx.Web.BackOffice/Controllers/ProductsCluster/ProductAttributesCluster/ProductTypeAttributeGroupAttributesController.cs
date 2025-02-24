using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Commands.CreateProductTypeAttributeGroupAttribute;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Commands.DeleteProductTypeAttributeGroupAttribute;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Commands.UpdateProductTypeAttributeGroupAttribute;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.BackOffice.GetProductTypeAttributeGroupAttribute;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.BackOffice.GetProductTypeAttributeGroupAttributes;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.Export.ExportExcelProductTypeAttributeGroupAttributes;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster.ProductAttributesCluster;


public class ProductTypeAttributeGroupAttributesController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllProductTypeAttributeGroupAttributeDto>>> GetAllProductTypeAttributeGroupAttributes()
    {
        return await Mediator.Send(new GetAllProductTypeAttributeGroupAttributesQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProductTypeAttributeGroupAttributeDto?>> GetProductTypeAttributeGroupAttributeById(int id)
    {
        return await Mediator.Send(new GetProductTypeAttributeGroupAttributeByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductTypeAttributeGroupAttributesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductTypeAttributeGroupAttributes.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductTypeAttributeGroupAttributeCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateProductTypeAttributeGroupAttributeCommand command)
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
        await Mediator.Send(new DeleteProductTypeAttributeGroupAttributeCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductTypeAttributeGroupAttributesCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
