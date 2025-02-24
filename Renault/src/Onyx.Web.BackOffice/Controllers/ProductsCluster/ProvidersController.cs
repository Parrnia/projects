using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.Providers.Commands.CreateProvider;
using Onyx.Application.Main.ProductsCluster.Providers.Commands.DeleteProvider;
using Onyx.Application.Main.ProductsCluster.Providers.Commands.UpdateProvider;
using Onyx.Application.Main.ProductsCluster.Providers.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.Providers.Queries.BackOffice.GetProvider;
using Onyx.Application.Main.ProductsCluster.Providers.Queries.BackOffice.GetProviders;
using Onyx.Application.Main.ProductsCluster.Providers.Queries.BackOffice.GetProvidersWithPagination;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.Providers.Validators;
using Onyx.Application.Main.ProductsCluster.Providers.Queries.Export.ExportExcelProviders;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;


public class ProvidersController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredProviderDto>> GetProvidersWithPagination([FromQuery] GetProvidersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllProviderDropDownDto>>> GetAllProvidersDropDown()
    {
        return await Mediator.Send(new GetAllProvidersDropDownQuery());
    }

    [HttpGet("allDropDown")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<ProviderDto>>> GetAllProviders()
    {
        return await Mediator.Send(new GetAllProvidersQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<ProviderDto?>> GetProviderById(int id)
    {
        return await Mediator.Send(new GetProviderByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelProvidersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Providers.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }


    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateProviderCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateProviderCommand command)
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
        await Mediator.Send(new DeleteProviderCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeProvider([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeProviderCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProviderLocalizedName([FromQuery] UniqueProviderLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueProviderName([FromQuery] UniqueProviderNameValidator query)
    {
        return await Mediator.Send(query);
    }
}
