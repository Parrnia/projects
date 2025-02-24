using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.InfoCluster.Countries.Commands.CreateCountry;
using Onyx.Application.Main.InfoCluster.Countries.Commands.DeleteCountry;
using Onyx.Application.Main.InfoCluster.Countries.Commands.UpdateCountry;
using Onyx.Application.Main.InfoCluster.Countries.Queries.BackOffice;
using Onyx.Application.Main.InfoCluster.Countries.Queries.BackOffice.GetCountries;
using Onyx.Application.Main.InfoCluster.Countries.Queries.BackOffice.GetCountriesWithPagination;
using Onyx.Application.Main.InfoCluster.Countries.Queries.BackOffice.GetCountry;
using Onyx.Application.Main.InfoCluster.Countries.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.InfoCluster.Countries.Queries.Export.ExportExcelCountries;

namespace Onyx.Web.BackOffice.Controllers.InfoCluster;


public class CountriesController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<CountryDto>>> GetCountriesWithPagination([FromQuery] GetCountriesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<CountryDto>>> GetAllCountries()
    {
        return await Mediator.Send(new GetAllCountriesQuery());
    }

    [HttpGet("allDropDown")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<AllCountryDropDownDto>>> GetAllCountriesDropDown()
    {
        return await Mediator.Send(new GetAllCountriesDropDownQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<CountryDto?>> GetCountryById(int id)
    {
        return await Mediator.Send(new GetCountryByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelCountriesQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Countries.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateCountryCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateCountryCommand command)
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
        await Mediator.Send(new DeleteCountryCommand(id));

        return NoContent();
    }
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeCountry([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeCountryCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueLocalizedName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueCountryLocalizedName([FromQuery] UniqueCountryLocalizedNameValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueName")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueCountryName([FromQuery] UniqueCountryNameValidator query)
    {
        return await Mediator.Send(query);
    }
    
}
