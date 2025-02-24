using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.BrandsCluster.Models.Commands.CreateModel;
using Onyx.Application.Main.BrandsCluster.Models.Commands.DeleteModel;
using Onyx.Application.Main.BrandsCluster.Models.Commands.UpdateModel;
using Onyx.Application.Main.BrandsCluster.Models.Queries.BackOffice;
using Onyx.Application.Main.BrandsCluster.Models.Queries.BackOffice.GetModel;
using Onyx.Application.Main.BrandsCluster.Models.Queries.BackOffice.GetModels;
using Onyx.Application.Main.BrandsCluster.Models.Queries.BackOffice.GetModelsWithPagination;
using Onyx.Application.Main.BrandsCluster.Models.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.BrandsCluster.Models.Queries.Export.ExportExcelModels;

namespace Onyx.Web.BackOffice.Controllers.BrandsCluster;


public class ModelsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<ModelDto>>> GetModelsWithPagination([FromQuery] GetModelsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ModelDto>>> GetAllModels()
    {
        return await Mediator.Send(new GetAllModelsQuery());
    }

    [HttpGet("family{familyId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ModelDto>>> GetModelsWithFamilyId(int familyId)
    {
        return await Mediator.Send(new GetModelsByFamilyIdQuery(familyId));
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ModelDto?>> GetModelById(int id)
    {
        return await Mediator.Send(new GetModelByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelModelsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Models.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateModelCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateModelCommand command)
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
        await Mediator.Send(new DeleteModelCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeModel([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeModelCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueModelLocalizedName([FromQuery] UniqueModelLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueModelName([FromQuery] UniqueModelNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
