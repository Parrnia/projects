using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.Products.Commands.CreateProduct;
using Onyx.Application.Main.ProductsCluster.Products.Commands.DeleteProduct;
using Onyx.Application.Main.ProductsCluster.Products.Commands.UpdateProduct;
using Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.GetProduct;
using Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.GetProducts;
using Onyx.Application.Main.ProductsCluster.Products.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.GetProductsWithPagination;
using Onyx.Domain.Enums;
using Onyx.Application.Main.ProductsCluster.Products.Queries.Export.ExportExcelProducts;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;


public class ProductsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<BackOfficeProductDto>>> GetProductsWithPagination([FromQuery] GetProductsWithPaginationQuery query)
    {
        if (UserInfo != null)
        {
            query.CustomerTypeEnum = (CustomerTypeEnum)UserInfo.CustomerType;
        }
        else
        {
            query.CustomerTypeEnum = CustomerTypeEnum.Personal;
        }
        return await Mediator.Send(query);
    }
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductDto>>> GetAllProducts(GetAllProductsQuery command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("allProduct")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredProductDto>> GetAllBackOfficeProducts([FromQuery] GetAllBackOfficeProductsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("kind")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductDto>>> GetProductsByKindId(GetProductsByKindIdQuery command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("brand")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductDto>>> GetProductsByBrandId(GetProductsByBrandIdQuery command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("byId")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProductDto?>> GetProductById(GetProductByIdQuery command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Products.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }


    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateProductCommand command)
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
        await Mediator.Send(new DeleteProductCommand(id));

        return NoContent();
    }
    
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductsCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProductLocalizedName([FromQuery] UniqueProductLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProductName([FromQuery] UniqueProductNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
