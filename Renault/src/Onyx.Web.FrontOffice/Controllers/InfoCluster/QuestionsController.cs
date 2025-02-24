using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.InfoCluster.Questions.Queries.FrontOffice.GetQuestion;
using Onyx.Application.Main.InfoCluster.Questions.Queries.FrontOffice.GetQuestions;
using Onyx.Application.Main.InfoCluster.Questions.Queries.FrontOffice.GetQuestionsWithPagination;

namespace Onyx.Web.FrontOffice.Controllers.InfoCluster;


public class QuestionsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<QuestionWithPaginationDto>>> GetQuestionsWithPagination([FromQuery] GetQuestionsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AllQuestionDto>>> GetAllQuestions()
    {
        return await Mediator.Send(new GetAllQuestionsQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QuestionByIdDto?>> GetQuestionById(int id)
    {
        return await Mediator.Send(new GetQuestionByIdQuery(id));
    }
}
