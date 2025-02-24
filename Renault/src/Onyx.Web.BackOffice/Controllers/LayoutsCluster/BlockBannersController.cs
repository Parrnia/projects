using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.LayoutsCluster.BlockBanners.Commands.CreateBlockBanner;
using Onyx.Application.Main.LayoutsCluster.BlockBanners.Commands.DeleteBlockBanner;
using Onyx.Application.Main.LayoutsCluster.BlockBanners.Commands.UpdateBlockBanner;
using Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.BackOffice;
using Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.BackOffice.GetBlockBanner;
using Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.BackOffice.GetBlockBanners;
using Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.BackOffice.GetBlockBannersWithPagination;
using Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.Export.ExportExcelBlockBanners;
using Onyx.Application.Main.LayoutsCluster.BlockBanners.Validators;
using OnyxAuth.Shared;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;

namespace Onyx.Web.BackOffice.Controllers.LayoutsCluster;

public class BlockBannersController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<BlockBannerDto>>> GetBlockBannersWithPagination([FromQuery] GetBlockBannersWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<BlockBannerDto>>> GetAllBlockBanners()
    {
        return await Mediator.Send(new GetAllBlockBannersQuery());
    }


    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<BlockBannerDto?>> GetBlockBannerById(int id)
    {
        return await Mediator.Send(new GetBlockBannerByIdQuery(id));
    }


    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelBlockBannersQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "BlockBanners.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateBlockBannerCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{urlId}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int urlId, UpdateBlockBannerCommand command)
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
        await Mediator.Send(new DeleteBlockBannerCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeBlockBanner([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeBlockBannerCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniquePosition")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueBlockBannerPosition([FromQuery] UniqueBlockBannerPositionValidator query)
    {
        return await Mediator.Send(query);
    }
}
