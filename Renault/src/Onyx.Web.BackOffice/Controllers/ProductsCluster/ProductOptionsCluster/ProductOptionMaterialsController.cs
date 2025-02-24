using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Commands.CreateProductOptionMaterial;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Commands.UpdateProductOptionMaterial;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.BackOffice.GetProductOptionMaterial;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.BackOffice.GetProductOptionMaterials;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Commands.DeleteProductOptionMaterial;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Material.ProductOptionMaterials.Queries.Export.ExportExcelProductOptionMaterials;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster.ProductOptionsCluster;

public class ProductOptionMaterialsController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductOptionMaterialDto>>> GetAllProductOptionMaterials()
    {
        return await Mediator.Send(new GetAllProductOptionMaterialsQuery());
    }

    [HttpGet("allDropDown")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllProductOptionMaterialDropDownDto>>> GetAllProductOptionMaterialsDropDown()
    {
        return await Mediator.Send(new GetAllProductOptionMaterialsDropDownQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProductOptionMaterialDto?>> GetProductOptionMaterialById(int id)
    {
        return await Mediator.Send(new GetProductOptionMaterialByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductOptionMaterialsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductOptionMaterials.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductOptionMaterialCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateProductOptionMaterialCommand command)
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
        await Mediator.Send(new DeleteProductOptionMaterialCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductOptionMaterialCommand() { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
