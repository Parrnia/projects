using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.ProductStatuses.Commands.CreateProductStatus;
using Onyx.Application.Main.ProductsCluster.ProductStatuses.Commands.DeleteProductStatus;
using Onyx.Application.Main.ProductsCluster.ProductStatuses.Commands.UpdateProductStatus;
using Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.BackOffice.GetProductStatus;
using Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.BackOffice.GetProductStatuses;
using Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.BackOffice.GetProductStatusesWithPagination;
using Onyx.Application.Main.ProductsCluster.ProductStatuses.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.Export.ExportExcelProductStatuses;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;


public class ProductStatusesController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<ProductStatusDto>>> GetProductStatusesWithPagination([FromQuery] GetProductStatusesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductStatusDto>>> GetAllProductStatuses()
    {
        return await Mediator.Send(new GetAllProductStatusesQuery());
    }

    [HttpGet("allDropDown")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllProductStatusDropDownDto>>> GetAllProductStatusesDropDown()
    {
        return await Mediator.Send(new GetAllProductStatusesDropDownQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProductStatusDto?>> GetProductStatusById(int id)
    {
        return await Mediator.Send(new GetProductStatusByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductStatusesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductStatuses.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }


    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductStatusCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateProductStatusCommand command)
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
        await Mediator.Send(new DeleteProductStatusCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeProductStatus([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductStatusCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProductStatusLocalizedName([FromQuery] UniqueProductStatusLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProductStatusName([FromQuery] UniqueProductStatusNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
