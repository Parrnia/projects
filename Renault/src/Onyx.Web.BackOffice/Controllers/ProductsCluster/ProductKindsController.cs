using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.ProductKinds.Commands;
using Onyx.Application.Main.ProductsCluster.ProductKinds.Queries.BackOffice;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.ProductKinds.Queries.Export.ExportExcelProductKinds;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;

public class ProductKindsController : ApiControllerBase
{


    
    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductKindsCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpGet("product{productId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllProductKindByProductIdDto>>> GetAllProductKindsByProductId(int productId)
    {
        return await Mediator.Send(new GetAllProductKindsByProductIdQuery(productId));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductKindsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductKinds.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }



    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductKindsCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPost("addRange")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> AddRange(AddRangeProductKindsCommand command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
