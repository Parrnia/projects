using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Main.ProductsCluster.Tags.Commands.CreateTag;
using Onyx.Application.Main.ProductsCluster.Tags.Commands.DeleteTag;
using Onyx.Application.Main.ProductsCluster.Tags.Commands.UpdateTag;
using Onyx.Application.Main.ProductsCluster.Tags.Queries.BackOffice;
using Onyx.Application.Main.ProductsCluster.Tags.Queries.BackOffice.GetTag;
using Onyx.Application.Main.ProductsCluster.Tags.Queries.BackOffice.GetTags;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.ProductsCluster.Tags.Queries.BackOffice.GetTagsWithPagination;
using Onyx.Application.Main.ProductsCluster.Tags.Validators;
using Onyx.Application.Main.ProductsCluster.Tags.Queries.Export.ExportExcelTags;

namespace Onyx.Web.BackOffice.Controllers.ProductsCluster;


public class TagsController : ApiControllerBase
{
    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<TagDto>>> GetAllTags()
    {
        return await Mediator.Send(new GetAllTagsQuery());
    }

    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<FilteredTagDto>> GetTagsWithPagination([FromQuery] GetTagsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<TagDto?>> GetTagById(int id)
    {
        return await Mediator.Send(new GetTagByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelTagsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Tags.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }


    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateTagCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateTagCommand command)
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
        await Mediator.Send(new DeleteTagCommand(id));

        return NoContent();
    }

    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeTag([FromBody] IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var command = new DeleteRangeTagCommand { Ids = ids };
        await Mediator.Send(command, cancellationToken);

        return NoContent();
    }


    //Validators
    [HttpGet("isUniqueTagEnTitle")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueTagEnTitleValidator([FromQuery] UniqueTagEnTitleValidator query)
    {
        return await Mediator.Send(query);
    }
    [HttpGet("isUniqueTagFaTitle")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueTagFaTitleValidator([FromQuery] UniqueTagFaTitleValidator query)
    {
        return await Mediator.Send(query);
    }
}
