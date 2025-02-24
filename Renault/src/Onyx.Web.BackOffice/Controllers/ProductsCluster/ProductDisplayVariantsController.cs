using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Commands.CreateVariant;
using Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Commands.DeleteVariant;
using Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Commands.UpdateVariant;
using Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Queries.BackOffice.GetVariant;
using Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Queries.BackOffice.GetVariants;
using Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Queries.Export.ExportExcelVariants;
using Onyx.Application.Main.ProductsCluster.ProductDisplayVariants.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;


public class ProductDisplayVariantsController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductDisplayVariantDto>>> GetAllProductDisplayVariants()
    {
        return await Mediator.Send(new GetAllVariantsQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProductDisplayVariantDto?>> GetProductDisplayVariantById(int id)
    {
        return await Mediator.Send(new GetVariantByIdQuery(id));
    }

    [HttpGet("product{productId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductDisplayVariantDto>>> GetAllProductDisplayVariantsByProductId(int productId)
    {
        return await Mediator.Send(new GetAllVariantsByProductIdQuery(productId));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelVariantsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductDisplayVariants.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateVariantCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateVariantCommand command)
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
        await Mediator.Send(new DeleteVariantCommand(id));

        return NoContent();
    }
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeProductDisplayVariant([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeVariantCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueProductDisplayVariantName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProductDisplayVariantName([FromQuery] UniqueVariantNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
