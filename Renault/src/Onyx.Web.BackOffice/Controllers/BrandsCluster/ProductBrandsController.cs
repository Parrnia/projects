using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.BrandsCluster.ProductBrands.Commands.CreateProductBrand;
using Onyx.Application.Main.BrandsCluster.ProductBrands.Commands.DeleteProductBrand;
using Onyx.Application.Main.BrandsCluster.ProductBrands.Commands.UpdateProductBrand;
using Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.BackOffice;
using Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.BackOffice.GetProductBrand;
using Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.BackOffice.GetProductBrands;
using Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.Export.ExportExcelProductBrands;
using Onyx.Application.Main.BrandsCluster.ProductBrands.Validators;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.BrandsCluster;


public class ProductBrandsController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductBrandDto>>> GetAllProductBrands()
    {
        return await Mediator.Send(new GetAllProductBrandsQuery());
    }

    [HttpGet("allDropDown")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllProductBrandDropDownDto>>> GetAllProductBrandsDropDown()
    {
        return await Mediator.Send(new GetAllProductBrandsDropDownQuery());
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductBrandsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductBrands.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProductBrandDto?>> GetProductBrandById(int id)
    {
        return await Mediator.Send(new GetProductBrandByIdQuery(id));
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductBrandCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int urlId, UpdateProductBrandCommand command)
    {
        if (urlId != command.Id)
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
        await Mediator.Send(new DeleteProductBrandCommand(id));

        return NoContent();
    }
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeProductBrand([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductBrandCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProductBrandLocalizedName([FromQuery] UniqueProductBrandLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProductBrandName([FromQuery] UniqueProductBrandNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
