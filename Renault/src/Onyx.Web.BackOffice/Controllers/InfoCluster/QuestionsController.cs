using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.InfoCluster.Questions.Commands.CreateQuestion;
using Onyx.Application.Main.InfoCluster.Questions.Commands.DeleteQuestion;
using Onyx.Application.Main.InfoCluster.Questions.Commands.UpdateQuestion;
using Onyx.Application.Main.InfoCluster.Questions.Queries.BackOffice;
using Onyx.Application.Main.InfoCluster.Questions.Queries.BackOffice.GetQuestion;
using Onyx.Application.Main.InfoCluster.Questions.Queries.BackOffice.GetQuestions;
using Onyx.Application.Main.InfoCluster.Questions.Queries.BackOffice.GetQuestionsWithPagination;
using Onyx.Application.Main.InfoCluster.Questions.Validators;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared.Permissions;
using OnyxAuth.Shared;
using Onyx.Application.Main.InfoCluster.Questions.Queries.Export.ExportExcelQuestions;

namespace Onyx.Web.BackOffice.Controllers.InfoCluster;


public class QuestionsController : ApiControllerBase
{
    [HttpGet]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<PaginatedList<QuestionDto>>> GetQuestionsWithPagination([FromQuery] GetQuestionsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<List<QuestionDto>>> GetAllQuestions()
    {
        return await Mediator.Send(new GetAllQuestionsQuery());
    }

    [HttpGet("{id}")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<QuestionDto?>> GetQuestionById(int id)
    {
        return await Mediator.Send(new GetQuestionByIdQuery(id));
    }

    [HttpGet("exportExcel")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> ExportExcelQuery([FromQuery] ExportExcelQuestionsQuery query)
    {
        var data = await Mediator.Send(query);
        string fileName = "Questions.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(data, contentType, fileName);
    }

    [HttpPost]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<int>> Create(CreateQuestionCommand command)
    {
        return await Mediator.Send(command);
    }
    [HttpPut("{id}")]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult> Update(int id, UpdateQuestionCommand command)
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
        await Mediator.Send(new DeleteQuestionCommand(id));

        return NoContent();
    }
    [HttpDelete]
    [CheckPermission(Roles.Employee, UserPermissions.BaseInformationManagement)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<IActionResult> DeleteRangeQuestion([FromBody] IEnumerable<int> ids)
    {
        var command = new DeleteRangeQuestionCommand { Ids = ids };
        await Mediator.Send(command);

        return NoContent();
    }

    //Validators
    [HttpGet("isUniqueQuestionText")]
    [CheckPermission(Roles.Employee, null)]
    [CheckPermission(Roles.SysAdmin, null)]
    public async Task<ActionResult<bool>> IsUniqueQuestionText([FromQuery] UniqueQuestionTextValidator query)
    {
        return await Mediator.Send(query);
    }
    
}
