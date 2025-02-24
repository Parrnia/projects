using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Commands.CreateProductTypeAttributeGroup;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Commands.DeleteProductTypeAttributeGroup;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Commands.UpdateProductTypeAttributeGroup;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.BackOffice.GetProductTypeAttributeGroup;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.BackOffice.GetProductTypeAttributeGroups;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Validators;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.Export.ExportExcelProductTypeAttributeGroups;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster.ProductAttributesCluster;


public class ProductTypeAttributeGroupsController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductTypeAttributeGroupDto>>> GetAllProductTypeAttributeGroups()
    {
        return await Mediator.Send(new GetAllProductTypeAttributeGroupsQuery());
    }

    [HttpGet("allDropDown")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllProductTypeAttributeGroupDropDownDto>>> GetAllProductTypeAttributeGroupsDropDown()
    {
        return await Mediator.Send(new GetAllProductTypeAttributeGroupsDropDownQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProductTypeAttributeGroupDto?>> GetProductTypeAttributeGroupById(int id)
    {
        return await Mediator.Send(new GetProductTypeAttributeGroupByIdQuery(id));
    }

    [HttpGet("ProductAttributeType{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductTypeAttributeGroupByProductAttributeTypeIdDto>>> GetProductTypeAttributeGroupsByProductAttributeTypeId(int id)
    {
        return await Mediator.Send(new GetProductTypeAttributeGroupsByProductAttributeTypeIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductTypeAttributeGroupsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductTypeAttributeGroups.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }


    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductTypeAttributeGroupCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateProductTypeAttributeGroupCommand command)
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
        await Mediator.Send(new DeleteProductTypeAttributeGroupCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductTypeAttributeGroupsCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProductTypeAttributeGroupName([FromQuery] UniqueProductTypeAttributeGroupNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
