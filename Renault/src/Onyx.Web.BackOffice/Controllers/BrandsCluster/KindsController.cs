using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.BrandsCluster.Kinds.Commands.CreateKind;
using Onyx.Application.Main.BrandsCluster.Kinds.Commands.DeleteKind;
using Onyx.Application.Main.BrandsCluster.Kinds.Commands.UpdateKind;
using Onyx.Application.Main.BrandsCluster.Kinds.Queries.BackOffice;
using Onyx.Application.Main.BrandsCluster.Kinds.Queries.BackOffice.GetKind;
using Onyx.Application.Main.BrandsCluster.Kinds.Queries.BackOffice.GetKinds;
using Onyx.Application.Main.BrandsCluster.Kinds.Queries.BackOffice.GetKindsWithPagination;
using Onyx.Application.Main.BrandsCluster.Kinds.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.BrandsCluster.Kinds.Queries.Export.ExportExcelKinds;

namespace Onyx.Web.BackOffice.Controllers.BrandsCluster;


public class KindsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<KindDto>>> GetKindsWithPagination([FromQuery] GetKindsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<KindDto>>> GetAllKinds()
    {
        return await Mediator.Send(new GetAllKindsQuery());
    }

    [HttpGet("allDropDown")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllKindDropDownDto>>> GetAllKindsDropDown()
    {
        return await Mediator.Send(new GetAllKindsDropDownQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<KindDto?>> GetKindById(int id)
    {
        return await Mediator.Send(new GetKindByIdQuery(id));
    }

    //[HttpGet("vin{vinNumber}")]
    //public async Task<ActionResult<KindForVinDto?>> GetKindByVinNumber(string vinNumber)
    //{
    //    return await Mediator.Send(new GetKindByVinNumberQuery(vinNumber));
    //}

    [HttpGet("model{modelId}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<KindDto>>> GetKindsWithModelId(int modelId)
    {
        return await Mediator.Send(new GetKindsByModelIdQuery(modelId));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelKindsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Kinds.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateKindCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateKindCommand command)
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
        await Mediator.Send(new DeleteKindCommand(id));

        return NoContent();
    }
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeKinds([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeKindsCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueKindLocalizedName([FromQuery] UniqueKindLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueKindName([FromQuery] UniqueKindNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
