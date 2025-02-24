using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.ProductTypes.Commands.CreateProductType;
using Onyx.Application.Main.ProductsCluster.ProductTypes.Commands.DeleteProductType;
using Onyx.Application.Main.ProductsCluster.ProductTypes.Commands.UpdateProductType;
using Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.BackOffice.GetProductType;
using Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.BackOffice.GetProductTypes;
using Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.BackOffice.GetProductTypesWithPagination;
using Onyx.Application.Main.ProductsCluster.ProductTypes.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.Export.ExportExcelProductTypes;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;


public class ProductTypesController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<ProductTypeDto>>> GetProductTypesWithPagination([FromQuery] GetProductTypesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductTypeDto>>> GetAllProductTypes()
    {
        return await Mediator.Send(new GetAllProductTypesQuery());
    }

    [HttpGet("allDropDown")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllProductTypeDropDownDto>>> GetAllProductTypesDropDown()
    {
        return await Mediator.Send(new GetAllProductTypesDropDownQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProductTypeDto?>> GetProductTypeById(int id)
    {
        return await Mediator.Send(new GetProductTypeByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductTypesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductTypes.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }


    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductTypeCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateProductTypeCommand command)
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
        await Mediator.Send(new DeleteProductTypeCommand(id));

        return NoContent();
    }
    
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductTypesCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProductTypeLocalizedName([FromQuery] UniqueProductTypeLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProductTypeName([FromQuery] UniqueProductTypeNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
