using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Commands.CreateProductAttributeOptionRole;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Commands.DeleteProductAttributeOptionRole;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Commands.UpdateProductAttributeOptionRole;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.BackOffice.GetProductAttributeOptionRole;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.BackOffice.GetProductAttributeOptionRoles;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.BackOffice.GetProductAttributeOptionRolesWithPagination;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Commands.DeleteProductAttributeOption;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.Export.ExportExcelProductAttributeOptionRoles;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster.ProductOptionsCluster;

public class ProductAttributeOptionRolesController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<ProductAttributeOptionRoleDto>>> GetProductAttributeOptionRolesWithPagination([FromQuery] GetProductAttributeOptionRolesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductAttributeOptionRoleDto>>> GetAllProductAttributeOptionRoles()
    {
        return await Mediator.Send(new GetAllProductAttributeOptionRolesQuery());
    }

    [HttpGet("option{optionId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductAttributeOptionRoleDto>>> GetAllProductAttributeOptionRoleByOptionId(int optionId)
    {
        return await Mediator.Send(new GetProductAttributeOptionRolesByProductAttributeOptionIdQuery(optionId));
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProductAttributeOptionRoleDto?>> GetProductAttributeOptionRoleById(int id)
    {
        return await Mediator.Send(new GetProductAttributeOptionRoleByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductAttributeOptionRolesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductAttributeOptionRoles.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }


    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductAttributeOptionRoleCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateProductAttributeOptionRoleCommand command)
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
        await Mediator.Send(new DeleteProductAttributeOptionRoleCommand(id));

        return NoContent();
    }
    
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductAttributeOptionRolesCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
