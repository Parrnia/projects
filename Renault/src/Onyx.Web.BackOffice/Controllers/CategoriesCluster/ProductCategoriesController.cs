using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Commands.CreateProductCategory;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Commands.DeleteProductCategory;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Commands.UpdateProductCategory;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.BackOffice;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.BackOffice.GetProductCategories;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.BackOffice.GetProductCategoriesWithPagination;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.BackOffice.GetProductCategory;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.Export.ExportExcelProductCategories;

namespace Onyx.Web.BackOffice.Controllers.CategoriesCluster;

public class ProductCategoriesController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductCategoryDto>>> GetAllProductCategories()
    {
        return await Mediator.Send(new GetAllProductCategoriesQuery());
    }

    [HttpGet("allDropDown")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllProductCategoryDropDownDto>>> GetAllProductCategoriesDropDown()
    {
        return await Mediator.Send(new GetAllProductCategoriesDropDownQuery());
    }

    [HttpGet("allFirstLayer")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductCategoryDto>>> GetAllFirstLayerProductCategories()
    {
        return await Mediator.Send(new GetAllFirstLayerCategoriesQuery());
    }

    [HttpGet("allWithPagination")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<ProductCategoryDto>>> GetProductCategoriesWithPagination([FromQuery] GetProductCategoriesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("productParentCategory")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductCategoryDto>>> GetProductCategoriesByProductParentCategoryId([FromQuery] GetProductCategoriesByProductParentCategoryIdQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProductCategoryDto?>> GetProductCategoryById(int id)
    {
        return await Mediator.Send(new GetProductCategoryByIdQuery(id));
    }

    [HttpGet("slug")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProductCategoryDto?>> GetProductCategoryBySlug([FromQuery] GetProductCategoryBySlugQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductCategoriesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductCategories.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductCategoryCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int urlId, UpdateProductCategoryCommand command)
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
        await Mediator.Send(new DeleteProductCategoryCommand(id));

        return NoContent();
    }
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeProductCategory([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductCategoryCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }
    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProductCategoryLocalizedName([FromQuery] UniqueProductCategoryLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProductCategoryName([FromQuery] UniqueProductCategoryNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
