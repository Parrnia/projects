using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.LayoutsCluster.Carousels.Commands.CreateCarousel;
using Onyx.Application.Main.LayoutsCluster.Carousels.Commands.DeleteCarousel;
using Onyx.Application.Main.LayoutsCluster.Carousels.Commands.UpdateCarousel;
using Onyx.Application.Main.LayoutsCluster.Carousels.Queries.BackOffice;
using Onyx.Application.Main.LayoutsCluster.Carousels.Queries.BackOffice.GetCarousel;
using Onyx.Application.Main.LayoutsCluster.Carousels.Queries.BackOffice.GetCarousels;
using Onyx.Application.Main.LayoutsCluster.Carousels.Queries.BackOffice.GetCarouselsWithPagination;
using Onyx.Application.Main.LayoutsCluster.Carousels.Queries.Export.ExportExcelCarousels;
using Onyx.Application.Main.LayoutsCluster.Carousels.Validators;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.LayoutsCluster;

public class CarouselsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<CarouselDto>>> GetCarouselsWithPagination([FromQuery] GetCarouselsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<CarouselDto>>> GetAllCarousels()
    {
        return await Mediator.Send(new GetAllCarouselsQuery());
    }


    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<CarouselDto?>> GetCarouselById(int id)
    {
        return await Mediator.Send(new GetCarouselByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelCarouselsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Carousels.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateCarouselCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int urlId, UpdateCarouselCommand command)
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
        await Mediator.Send(new DeleteCarouselCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeCarousel([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeCarouselCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueTitle")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueCarouselTitle([FromQuery] UniqueCarouselTitleValidator query)
    {
        return await Mediator.Send(query);
    }
}
