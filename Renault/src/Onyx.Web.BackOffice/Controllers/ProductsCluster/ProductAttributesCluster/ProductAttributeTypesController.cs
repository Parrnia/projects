using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.CreateProductAttributeType;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.DeleteProductAttributeType;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.UpdateProductAttributeType;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice.GetProductAttributeType;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.BackOffice.GetProductAttributeTypes;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Validators;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroupAttributes.Queries.BackOffice.GetProductTypeAttributeGroupAttributes;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.AddProductAttributeGroups;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.RemoveProductAttributeGroups;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Queries.BackOffice.GetProductTypeAttributeGroups;
using Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Queries.Export.ExportExcelProductAttributeTypes;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster.ProductAttributesCluster;


public class ProductAttributeTypesController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductAttributeTypeDto>>> GetAllProductAttributeTypes()
    {
        return await Mediator.Send(new GetAllProductAttributeTypesQuery());
    }

    [HttpGet("allDropDown")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllProductAttributeTypeDropDownDto>>> GetAllProductAttributeTypesDropDown()
    {
        return await Mediator.Send(new GetAllProductAttributeTypesDropDownQuery());
    }

    [HttpGet("allGroup")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductTypeAttributeGroupDto>>> GetAllProductAttributeTypesGroup()
    {
        return await Mediator.Send(new GetAllProductTypeAttributeGroupsQuery());
    }

    [HttpGet("group{groupId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProductTypeAttributeGroupAttributeDto>>> GetAllProductTypeAttributeGroupAttributeByGroupId(int groupId)
    {
        return await Mediator.Send(new GetAllProductTypeAttributeGroupAttributeByGroupQuery(groupId));
    }


    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProductAttributeTypeDto?>> GetProductAttributeTypeById(int id)
    {
        return await Mediator.Send(new GetProductAttributeTypeByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProductAttributeTypesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "ProductAttributeTypes.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }


    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProductAttributeTypeCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("addProductAttributeGroups{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> AddProductAttributeGroups(int id, AddProductAttributeGroupsCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("removeProductAttributeGroups{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> RemoveProductAttributeGroups(int id, RemoveProductAttributeGroupsCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }


    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateProductAttributeTypeCommand command)
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
        await Mediator.Send(new DeleteProductAttributeTypeCommand(id));

        return NoContent();
    }
    

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProductAttributeTypesCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProductAttributeTypeName([FromQuery] UniqueProductAttributeTypeNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
